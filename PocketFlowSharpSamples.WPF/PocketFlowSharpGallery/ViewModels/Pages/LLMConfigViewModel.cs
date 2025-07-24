using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketFlowSharpGallery.Models;
using PocketFlowSharpGallery.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            }
            else
            {
                await _repository.UpdateAsync(Config);
            }

            await LoadConfigsAsync();
            Cancel();
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
        private async Task EditItemAsync(LLMConfig config)
        {
            if (config != null)
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
            }
        }

        [RelayCommand]
        private async Task DeleteItemAsync(LLMConfig config)
        {
            if (config != null)
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
    }
}