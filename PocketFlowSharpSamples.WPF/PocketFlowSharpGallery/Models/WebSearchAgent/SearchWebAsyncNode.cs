using PocketFlowSharp;
using PocketFlowSharpGallery.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Models.WebSearchAgent
{
    /// <summary>
    /// 异步搜索节点，执行网络搜索
    /// </summary>
    public class SearchWebAsyncNode : AsyncNode
    {
        private readonly IProgressReporter _progressReporter;

        public SearchWebAsyncNode(IProgressReporter progressReporter)
        {
            _progressReporter = progressReporter ?? throw new ArgumentNullException(nameof(progressReporter));
        }

        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            // 获取搜索查询
            string searchQuery = (string)shared["search_query"];
            _progressReporter.ReportProgress("search", $"Preparing search: {searchQuery}", 40);
            _progressReporter.ReportIntermediateResult("search", $"Search query: {searchQuery}");
            
            return searchQuery;
        }

        protected override async Task<object> ExecAsync(object searchQuery)
        {
            string query = searchQuery.ToString() ?? "";
            
            _progressReporter.ReportProgress("search", $"Searching web for: {query}", 50);
            _progressReporter.ReportIntermediateResult("search", $"Initiating web search...");

            try
            {
                // 执行异步搜索
                var results = await Utils.SearchWebAsync(query);
                
                _progressReporter.ReportProgress("search", "Search completed, processing results...", 70);
                _progressReporter.ReportIntermediateResult("search", 
                    $"Found {results?.ToString()?.Length ?? 0} characters of search results");

                return results;
            }
            catch (Exception ex)
            {
                _progressReporter.ReportError($"Search failed: {ex.Message}");
                throw;
            }
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // 保存搜索结果并准备返回决策节点
            string previous = shared.ContainsKey("context") ? (string)shared["context"] : "";
            string searchQuery = (string)shared["search_query"];
            string searchResults = execResult.ToString() ?? "";

            // 构建新的上下文
            string newContext = previous + "\n\nSEARCH: " + searchQuery + "\nRESULTS: " + searchResults;
            shared["context"] = newContext;

            _progressReporter.ReportProgress("search", "Analyzing search results...", 80);
            _progressReporter.ReportIntermediateResult("search", 
                $"Search results added to context ({searchResults.Length} characters)");

            // 总是返回决策节点进行下一步分析
            return "decide";
        }
    }
}