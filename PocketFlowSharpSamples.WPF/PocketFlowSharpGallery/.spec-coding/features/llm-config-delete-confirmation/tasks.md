# LLM配置删除确认功能实现任务清单

## 任务列表

### 1. 创建对话框服务接口和实现
- [ ] 创建 `Services/IDialogService.cs` 接口文件
  - 定义 `Task<bool> ShowDeleteConfirmationAsync(string configName)` 方法
  - 参考需求 EARS-001, EARS-004

- [ ] 创建 `Services/DialogService.cs` 实现文件
  - 实现 `ShowDeleteConfirmationAsync` 方法
  - 集成 WPF-UI 的 FluentWindow
  - 处理对话框生命周期

### 2. 创建确认对话框视图模型
- [ ] 创建 `ViewModels/Dialogs/DeleteConfirmationViewModel.cs`
  - 添加 `ConfigName` 属性 (参考 EARS-002)
  - 添加 `IsConfirmed` 属性
  - 实现 `ConfirmCommand` 和 `CancelCommand` (参考 EARS-011, EARS-012)
  - 支持键盘快捷键 (参考 EARS-013)

### 3. 创建确认对话框UI
- [ ] 创建 `Views/Dialogs/DeleteConfirmationDialog.xaml`
  - 使用 WPF-UI 的 FluentWindow
  - 实现警告图标和标题 (参考 EARS-006, EARS-007)
  - 显示配置名称和警告文本 (参考 EARS-002, EARS-003)
  - 配置按钮样式和焦点 (参考 EARS-009, EARS-010)
  - 设置窗口属性 (参考 EARS-005)

- [ ] 创建 `Views/Dialogs/DeleteConfirmationDialog.xaml.cs`
  - 实现代码隐藏逻辑
  - 处理窗口关闭事件

### 4. 修改现有ViewModel
- [ ] 修改 `ViewModels/Pages/LLMConfigViewModel.cs`
  - 添加 `IDialogService` 依赖注入
  - 修改构造函数接受 `IDialogService` 参数
  - 更新 `DeleteItemCommand` 实现 (参考 EARS-001, EARS-011, EARS-012)
  - 添加异常处理 (参考 EARS-015, EARS-017)

### 5. 配置依赖注入
- [ ] 修改 `App.xaml.cs` 或相关配置文件
  - 注册 `IDialogService` 到 `DialogService`
  - 确保 `LLMConfigViewModel` 正确解析依赖

### 6. 添加错误处理
- [ ] 在 `DeleteItemCommand` 中添加 try-catch 块
  - 处理 `DbUpdateException` (参考 EARS-017)
  - 显示用户友好的错误消息 (参考 EARS-015)
  - 记录异常日志

### 7. 用户体验优化
- [ ] 在 `DeleteItemCommand` 中添加成功提示
  - 删除成功后显示消息框 (参考 EARS-023)
  - 刷新配置列表 (参考 EARS-014)

### 8. 创建单元测试
- [ ] 创建 `Tests/ViewModels/DeleteConfirmationViewModelTests.cs`
  - 测试 `ConfirmCommand` 设置 `IsConfirmed = true`
  - 测试 `CancelCommand` 设置 `IsConfirmed = false`
  - 测试属性变更通知

- [ ] 创建 `Tests/Services/DialogServiceTests.cs`
  - 测试对话框服务返回正确结果
  - 测试异常处理

### 9. 集成测试
- [ ] 创建 `Tests/ViewModels/LLMConfigViewModelDeleteTests.cs`
  - 测试用户确认删除流程
  - 测试用户取消删除流程
  - 测试异常处理流程

### 10. 代码清理和验证
- [ ] 验证所有文件引用正确
- [ ] 检查命名空间一致性
- [ ] 确保没有编译警告
- [ ] 验证资源释放 (Dispose pattern if needed)

## 实现顺序建议
1. 先创建服务接口和实现 (任务 1)
2. 创建视图模型 (任务 2)
3. 创建UI组件 (任务 3)
4. 修改现有代码 (任务 4, 6, 7)
5. 配置依赖注入 (任务 5)
6. 编写测试 (任务 8, 9)
7. 最终验证 (任务 10)