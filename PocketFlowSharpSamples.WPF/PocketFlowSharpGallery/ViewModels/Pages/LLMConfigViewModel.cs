using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketFlowSharpGallery.Models;
using PocketFlowSharpGallery.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PocketFlowSharpGallery.ViewModels.Pages
{
    public partial class LLMConfigViewModel : ObservableObject
    {
        private readonly ILLMConfigRepository _repository;

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

        public LLMConfigViewModel(ILLMConfigRepository repository)
        {
            _repository = repository;
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
                await _repository.DeleteAsync(config.Id);
                await LoadConfigsAsync();
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