using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketFlowSharp;
using PocketFlowSharpGallery.Models.WebSearchAgent;
using PocketFlowSharpGallery.Models;
using PocketFlowSharpGallery.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.ViewModels.Pages
{
    public partial class WebSearchViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _question = "";

        [ObservableProperty]
        private string _result = "";

        [ObservableProperty]
        private bool _isSearching = false;

        [ObservableProperty]
        private string _searchStatus = "Ready";

        [ObservableProperty]
        private bool _canCancel = false;

        [ObservableProperty]
        private ObservableCollection<string> _searchHistory = new();

        // Configuration properties
        [ObservableProperty]
        private string _modelName = "";

        [ObservableProperty]
        private string _endPoint = "";

        [ObservableProperty]
        private string _apiKey = "";

        [ObservableProperty]
        private string _braveSearchApiKey = "";

        // Database configuration options
        [ObservableProperty]
        private ObservableCollection<LLMConfig> _llmConfigs = new();

        [ObservableProperty]
        private ObservableCollection<SearchEngineConfig> _searchEngineConfigs = new();

        [ObservableProperty]
        private LLMConfig _selectedLLMConfig;

        [ObservableProperty]
        private SearchEngineConfig _selectedSearchEngineConfig;

        private readonly ILLMConfigRepository _llmConfigRepository;
        private readonly ISearchEngineConfigRepository _searchEngineConfigRepository;
        private CancellationTokenSource _cancellationTokenSource;
        private IProgressReporter _progressReporter;

        public WebSearchViewModel(ILLMConfigRepository llmConfigRepository, ISearchEngineConfigRepository searchEngineConfigRepository)
        {
            _llmConfigRepository = llmConfigRepository;
            _searchEngineConfigRepository = searchEngineConfigRepository;
            
            LoadConfiguration();
            LoadDatabaseConfigs();
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(Question))
            {
                Result = "Please enter a question.";
                return;
            }

            if (!ValidateConfiguration())
            {
                Result = "Please configure API keys in the settings first.";
                return;
            }

            // 取消之前的操作
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            IsSearching = true;
            CanCancel = true;
            SearchStatus = "Initializing...";
            Result = "";

            try
            {
                // 创建进度报告器
                _progressReporter = new WpfProgressReporter(this);

                // 更新配置
                Utils.ModelName = ModelName;
                Utils.EndPoint = EndPoint;
                Utils.ApiKey = ApiKey;
                Utils.BraveSearchApiKey = BraveSearchApiKey;

                // 创建异步流程
                var flow = CreateAsyncFlow();

                // 处理问题的共享数据
                var shared = new Dictionary<string, object> { { "question", Question } };
                
                _progressReporter.PrintMessage("[初始化] 开始搜索流程...");
                
                // 运行异步流程
                await flow.RunAsync(shared);

                // 获取最终结果
                string answer = shared.ContainsKey("answer") ? shared["answer"].ToString() ?? "" : "No answer found";
                
                // 添加到历史记录
                SearchHistory.Insert(0, $"Q: {Question}\nA: {answer}\n");

                _progressReporter.PrintMessage(answer);
            }
            catch (OperationCanceledException)
            {
                Result = "Search cancelled by user";
                SearchStatus = "Cancelled";
            }
            catch (Exception ex)
            {
                Result = $"Error: {ex.Message}";
                SearchStatus = "Error occurred";
                _progressReporter?.PrintMessage($"[错误] {ex.Message}");
            }
            finally
            {
                IsSearching = false;
                CanCancel = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        [RelayCommand]
        private void CancelSearch()
        {
            _cancellationTokenSource?.Cancel();
        }

        [RelayCommand]
        private void ClearHistory()
        {
            SearchHistory.Clear();
        }

        [RelayCommand]
        private void ClearResult()
        {
            Result = "";
            SearchStatus = "Ready";
        }

        private AsyncFlow CreateAsyncFlow()
        {
            // 创建异步节点实例
            var decide = new DecideActionAsyncNode(_progressReporter);
            var search = new SearchWebAsyncNode(_progressReporter);
            var answer = new AnswerQuestionAsyncNode(_progressReporter);

            // 连接节点
            decide.Next(search, "search");
            decide.Next(answer, "answer");
            search.Next(decide, "decide");

            // 创建异步流程
            return new AsyncFlow(decide);
        }

        private bool ValidateConfiguration()
        {
            return !string.IsNullOrWhiteSpace(ModelName) &&
                   !string.IsNullOrWhiteSpace(EndPoint) &&
                   !string.IsNullOrWhiteSpace(ApiKey) &&
                   !string.IsNullOrWhiteSpace(BraveSearchApiKey);
        }

        private void LoadConfiguration()
        {
            ModelName = "gpt-3.5-turbo";
            EndPoint = "https://api.openai.com/v1";
            ApiKey = "your-openai-api-key";
            BraveSearchApiKey = "your-brave-search-api-key";
        }

        private async void LoadDatabaseConfigs()
        {
            try
            {
                // Load LLM configurations
                var llmConfigs = await _llmConfigRepository.GetAllAsync();
                LlmConfigs.Clear();
                foreach (var config in llmConfigs)
                {
                    LlmConfigs.Add(config);
                }

                // Load Search Engine configurations
                var searchEngineConfigs = await _searchEngineConfigRepository.GetAllAsync();
                SearchEngineConfigs.Clear();
                foreach (var config in searchEngineConfigs)
                {
                    SearchEngineConfigs.Add(config);
                }

                // Auto-select first available configs
                if (LlmConfigs.Any())
                {
                    SelectedLLMConfig = LlmConfigs.First();
                    ApplyLLMConfig(SelectedLLMConfig);
                }

                if (SearchEngineConfigs.Any())
                {
                    SelectedSearchEngineConfig = SearchEngineConfigs.First();
                    ApplySearchEngineConfig(SelectedSearchEngineConfig);
                }
            }
            catch (Exception ex)
            {
                SearchStatus = $"Error loading configurations: {ex.Message}";
            }
        }

        partial void OnSelectedLLMConfigChanged(LLMConfig value)
        {
            if (value != null)
            {
                ApplyLLMConfig(value);
            }
        }

        partial void OnSelectedSearchEngineConfigChanged(SearchEngineConfig value)
        {
            if (value != null)
            {
                ApplySearchEngineConfig(value);
            }
        }

        private void ApplyLLMConfig(LLMConfig config)
        {
            ModelName = config.ModelName;
            EndPoint = config.EndPoint;
            ApiKey = config.ApiKey;
        }

        private void ApplySearchEngineConfig(SearchEngineConfig config)
        {
            BraveSearchApiKey = config.ApiKey;
        }

        [RelayCommand]
        private void SaveConfiguration()
        {
            // Save configuration to app settings
            // This can be implemented later to persist settings
            SearchStatus = "Configuration saved";
        }

        [RelayCommand]
        private async Task RefreshConfigsAsync()
        {
            SearchStatus = "Refreshing configurations...";
            try
            {
                await Task.Run(LoadDatabaseConfigs);
                SearchStatus = "Configurations refreshed successfully";
            }
            catch (Exception ex)
            {
                SearchStatus = $"Error refreshing configurations: {ex.Message}";
            }
        }
    }
}
