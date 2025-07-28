using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketFlowSharpGallery.Models;
using PocketFlowSharpGallery.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace PocketFlowSharpGallery.ViewModels.Pages
{
    public partial class LLMConfigViewModel : ObservableObject
    {
        private readonly ILLMConfigRepository _repository;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private LLMConfig _config = new LLMConfig();

        [ObservableProperty]
        private ObservableCollection<LLMConfig> _configs = new ObservableCollection<LLMConfig>();

        [ObservableProperty]
        private bool _isEditing = false;

        [ObservableProperty]
        private bool _isNewConfig = true;

        [ObservableProperty]
        private int _selectedTabIndex = 0;
        
        [ObservableProperty]
        private bool _isApiKeyVisible = false;

        public LLMConfigViewModel(ILLMConfigRepository repository, IDialogService dialogService)
        {
            _repository = repository;
            _dialogService = dialogService;
        }

        public async Task InitializeAsync()
        {
            await LoadConfigsAsync();
        }

        [RelayCommand]
        private async Task LoadConfigsAsync()
        {
            var configs = await _repository.GetAllAsync();
            Configs.Clear();
            foreach (var config in configs)
            {
                Configs.Add(config);
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            // 验证必填字段
            if (string.IsNullOrWhiteSpace(Config.Provider))
            {
                MessageBox.Show("请填写提供商名称！", "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Config.EndPoint))
            {
                MessageBox.Show("请填写API端点地址！", "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Config.ModelName))
            {
                MessageBox.Show("请填写模型名称！", "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Config.ApiKey))
            {
                MessageBox.Show("请填写API密钥！", "验证错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (IsNewConfig)
                {
                    await _repository.AddAsync(Config);
                    MessageBox.Show("LLM配置已成功添加！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _repository.UpdateAsync(Config);
                    MessageBox.Show("LLM配置已成功更新！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                await LoadConfigsAsync();
                Cancel();
                SelectedTabIndex = 1; // 切换到Query Tab
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            Config = new LLMConfig();
            IsEditing = false;
            IsNewConfig = true;
        }

        [RelayCommand]
        private void Edit()
        {
            IsEditing = true;
        }

        [RelayCommand]
        private void AddNew()
        {
            Config = new LLMConfig();
            IsNewConfig = true;
            IsEditing = true;
        }

        [RelayCommand]
        private async Task EditItem(object parameter)
        {
            if (parameter is LLMConfig config && config != null)
            {
                // 检查要编辑的配置是否完整
                if (string.IsNullOrWhiteSpace(config.Provider) || 
                    string.IsNullOrWhiteSpace(config.EndPoint) || 
                    string.IsNullOrWhiteSpace(config.ModelName) || 
                    string.IsNullOrWhiteSpace(config.ApiKey))
                {
                    var result = MessageBox.Show(
                        "此配置缺少一些信息，继续编辑可能会导致问题。\n\n是否要继续编辑？", 
                        "配置不完整",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);
                    
                    if (result != MessageBoxResult.Yes)
                        return;
                }

                Config = new LLMConfig
                {
                    Id = config.Id,
                    Provider = config.Provider,
                    EndPoint = config.EndPoint,
                    ModelName = config.ModelName,
                    ApiKey = config.ApiKey
                };
                IsNewConfig = false;
                IsEditing = true;
                SelectedTabIndex = 0; // 切换到Edit Tab
            }
        }

        [RelayCommand]
        private async Task DeleteItem(object parameter)
        {
            if (parameter is LLMConfig config && config != null)
            {
                try
                {
                    // 显示删除确认对话框
                    var confirmed = await _dialogService.ShowDeleteConfirmationAsync("配置", config.Provider);
                    
                    if (confirmed)
                    {
                        await _repository.DeleteAsync(config.Id);
                        await LoadConfigsAsync();
                        
                        // 显示删除成功提示
                        MessageBox.Show($"配置 '{config.Provider}' 已成功删除！", "删除成功",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (DbUpdateException ex)
                {
                    // 处理数据库异常
                    MessageBox.Show($"删除配置时发生数据库错误：{ex.Message}", "删除失败",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    // 处理其他异常
                    MessageBox.Show($"删除配置时发生错误：{ex.Message}", "删除失败",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadConfigsAsync();
        }
        
        [RelayCommand]
        private void ToggleApiKeyVisibility()
        {
            IsApiKeyVisible = !IsApiKeyVisible;
        }
    }
}