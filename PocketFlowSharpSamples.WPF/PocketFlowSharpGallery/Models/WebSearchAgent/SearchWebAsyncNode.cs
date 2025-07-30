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
            
            return searchQuery;
        }

        protected override async Task<object> ExecAsync(object searchQuery)
        {
            string query = searchQuery.ToString() ?? "";
            
            _progressReporter.PrintMessage($"[搜索节点] 正在搜索: {query}");

            try
            {
                // 执行异步搜索
                var results = await Utils.SearchWebAsync(query);
                
                _progressReporter.PrintMessage($"[搜索节点] 搜索完成，找到 {results?.ToString()?.Length ?? 0} 字符的结果");

                return results;
            }
            catch (Exception ex)
            {
                _progressReporter.PrintMessage($"[搜索节点] 错误: 搜索失败 - {ex.Message}");
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

            // 总是返回决策节点进行下一步分析
            return "decide";
        }
    }
}