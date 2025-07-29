using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketFlowSharp;
using PocketFlowSharpGallery.Models.WebSearchAgent;
using PocketFlowSharpGallery.Models;
using PocketFlowSharpGallery.Services;
using System.Collections.ObjectModel;

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

            IsSearching = true;
            SearchStatus = "Searching...";
            Result = "";

            try
            {
                // Update Utils with current configuration
                Utils.ModelName = ModelName;
                Utils.EndPoint = EndPoint;
                Utils.ApiKey = ApiKey;
                Utils.BraveSearchApiKey = BraveSearchApiKey;

                // Create the agent flow
                Flow flow = CreateFlow();

                // Process the question
                var shared = new Dictionary<string, object> { { "question", Question } };
                
                SearchStatus = "Processing question...";

                flow.Run(shared);
                //await Task.Run(() => flow.Run(shared));

                // Get the result
                string answer = shared.ContainsKey("answer") ? shared["answer"].ToString() ?? "" : "No answer found";
                Result = answer;

                // Add to history
                SearchHistory.Insert(0, $"Q: {Question}\nA: {answer}\n");

                SearchStatus = "Completed";
            }
            catch (Exception ex)
            {
                Result = $"Error: {ex.Message}";
                SearchStatus = "Error occurred";
            }
            finally
            {
                IsSearching = false;
            }
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

        private Flow CreateFlow()
        {
            // Create instances of each node
            var decide = new DecideActionNode();
            var search = new SearchWebNode();
            var answer = new AnswerQuestionNode();

            // Connect the nodes
            // If DecideAction returns "search", go to SearchWeb
            decide.Next(search, "search");

            // If DecideAction returns "answer", go to AnswerQuestion
            decide.Next(answer, "answer");

            // After SearchWeb completes and returns "decide", go back to DecideAction
            search.Next(decide, "decide");

            // Create the flow, starting with the DecideAction node
            var flow = new Flow(decide);

            return flow;
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
