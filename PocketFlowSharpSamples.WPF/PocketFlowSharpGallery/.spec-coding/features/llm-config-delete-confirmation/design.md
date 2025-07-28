# LLM配置删除确认功能设计文档

## 概述
本设计文档描述了为PocketFlowSharpGallery应用添加LLM配置删除确认功能的完整技术方案。该功能将在用户尝试删除LLM配置时显示确认对话框，防止意外删除操作。

## 架构设计

### 整体架构
采用MVVM模式，在现有架构基础上添加以下组件：
- **视图层**: 确认对话框窗口
- **视图模型层**: 确认对话框ViewModel
- **服务层**: 对话框服务接口
- **交互逻辑**: 修改现有DeleteItemCommand

### 技术栈
- **框架**: WPF with .NET 8
- **UI库**: WPF-UI (lepo.co/wpfui)
- **MVVM**: CommunityToolkit.Mvvm
- **数据访问**: Entity Framework Core

## 组件设计

### 1. 确认对话框 (DeleteConfirmationDialog.xaml)
```xml
<ui:FluentWindow x:Class="PocketFlowSharpGallery.Views.Dialogs.DeleteConfirmationDialog"
                 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                 xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
                 Title="确认删除"
                 Width="400"
                 Height="200"
                 WindowStartupLocation="CenterOwner"
                 ResizeMode="NoResize">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- 标题和图标 -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="0,0,0,15">
            <ui:SymbolIcon Symbol="Warning24" Foreground="#FFBF00" Width="24" Height="24" Margin="0,0,10,0"/>
            <TextBlock Text="确认删除" FontSize="18" FontWeight="SemiBold"/>
        </StackPanel>
        
        <!-- 确认消息 -->
        <TextBlock Grid.Row="1" TextWrapping="Wrap">
            <Run Text="您确定要删除LLM配置"/>
            <Run Text="{Binding ConfigName}" FontWeight="Bold"/>
            <Run Text="吗？"/>
            <LineBreak/>
            <Run Text="此操作不可撤销，删除后配置将无法恢复。" Foreground="Red"/>
        </TextBlock>
        
        <!-- 按钮 -->
        <StackPanel Grid.Row="2" Orientation="Horizontal" HorizontalAlignment="Right">
            <Button Content="取消" 
                    Command="{Binding CancelCommand}"
                    IsCancel="True"
                    Margin="5"
                    Padding="20,5"/>
            <Button Content="确定" 
                    Command="{Binding ConfirmCommand}"
                    IsDefault="True"
                    Margin="5"
                    Padding="20,5"
                    Background="#DC3545"
                    Foreground="White"/>
        </StackPanel>
    </Grid>
</ui:FluentWindow>
```

### 2. 确认对话框ViewModel (DeleteConfirmationViewModel.cs)
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace PocketFlowSharpGallery.ViewModels.Dialogs
{
    public partial class DeleteConfirmationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _configName = string.Empty;

        [ObservableProperty]
        private bool _isConfirmed = false;

        [RelayCommand]
        private void Confirm()
        {
            IsConfirmed = true;
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
        }

        [RelayCommand]
        private void Cancel()
        {
            IsConfirmed = false;
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
        }
    }
}
```

### 3. 对话框服务接口 (IDialogService.cs)
```csharp
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Services
{
    public interface IDialogService
    {
        Task<bool> ShowDeleteConfirmationAsync(string configName);
    }
}

public class DialogService : IDialogService
{
    public async Task<bool> ShowDeleteConfirmationAsync(string configName)
    {
        var dialog = new DeleteConfirmationDialog
        {
            DataContext = new DeleteConfirmationViewModel { ConfigName = configName }
        };
        
        dialog.ShowDialog();
        
        if (dialog.DataContext is DeleteConfirmationViewModel vm)
        {
            return vm.IsConfirmed;
        }
        
        return false;
    }
}
```

### 4. 修改LLMConfigViewModel
```csharp
// 添加对话框服务依赖
private readonly ILLMConfigRepository _repository;
private readonly IDialogService _dialogService;

public LLMConfigViewModel(ILLMConfigRepository repository, IDialogService dialogService)
{
    _repository = repository;
    _dialogService = dialogService;
}

// 修改DeleteItemCommand
[RelayCommand]
private async Task DeleteItem(object parameter)
{
    if (parameter is LLMConfig config && config != null)
    {
        // 显示确认对话框
        var confirmed = await _dialogService.ShowDeleteConfirmationAsync(config.Provider);
        
        if (confirmed)
        {
            try
            {
                await _repository.DeleteAsync(config.Id);
                await LoadConfigsAsync();
                
                // 显示成功提示
                MessageBox.Show("配置已成功删除！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除配置时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
```

## 数据模型

### 现有模型保持不变
- **LLMConfig**: 现有的配置实体模型
- **ILLMConfigRepository**: 现有的数据访问接口

### 新增模型
- **DeleteConfirmationViewModel**: 确认对话框的视图模型

## 错误处理

### 异常处理策略
1. **网络异常**: 显示友好的错误提示
2. **数据库异常**: 记录日志并显示用户友好的消息
3. **并发冲突**: 检测并提示用户数据已变更
4. **权限异常**: 提示用户权限不足

### 错误消息映射
```csharp
private static readonly Dictionary<Type, string> ErrorMessages = new()
{
    [typeof(DbUpdateException)] = "数据库操作失败，请稍后重试",
    [typeof(TimeoutException)] = "操作超时，请检查网络连接",
    [typeof(UnauthorizedAccessException)] = "权限不足，无法执行此操作"
};
```

## 测试策略

### 单元测试
1. **DeleteConfirmationViewModel测试**
   - 确认命令设置IsConfirmed为true
   - 取消命令设置IsConfirmed为false
   - 配置名称正确绑定

2. **DialogService测试**
   - 对话框正确显示配置名称
   - 返回用户选择结果

3. **LLMConfigViewModel测试**
   - 用户确认时执行删除
   - 用户取消时不执行删除
   - 异常处理正确

### 集成测试
1. **UI交互测试**
   - 对话框显示和关闭行为
   - 键盘快捷键支持
   - 焦点管理

2. **数据一致性测试**
   - 删除后数据列表更新
   - 错误状态恢复

### UI测试
1. **视觉测试**
   - 对话框样式一致性
   - 响应式布局
   - 高DPI支持

## 部署考虑

### 依赖注入配置
```csharp
// 在App.xaml.cs中注册服务
services.AddSingleton<IDialogService, DialogService>();
```

### 向后兼容性
- 保持现有API不变
- 新增服务可选注入
- 支持渐进式升级

## 性能考虑

### 优化措施
1. **异步操作**: 所有I/O操作使用async/await
2. **资源管理**: 及时释放对话框资源
3. **内存优化**: 避免大对象在UI线程分配

### 性能指标
- 对话框显示时间 < 100ms
- 删除操作响应时间 < 1s
- 内存使用增量 < 1MB

## 安全考虑

### 数据保护
- 不记录敏感配置信息到日志
- 确认对话框不显示API密钥
- 使用安全字符串处理敏感数据

### 审计日志
```csharp
public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Details { get; set; } = string.Empty;
}