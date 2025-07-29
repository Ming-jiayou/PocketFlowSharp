using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Search_Agent
{
    public class SearchWebNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            // Get the search query from the shared store
            return shared["search_query"];
        }

        protected override async Task<object> ExecAsync(object searchQuery)
        {
            Console.WriteLine($"🌐 Searching the web for: {searchQuery}");
            string query = searchQuery.ToString();
            var results = await Utils.SearchWebAsync(query);
            return results;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // Save the search results and go back to the decision node
            string previous = shared.ContainsKey("context") ? (string)shared["context"] : "";
            shared["context"] = previous + "\n\nSEARCH: " + shared["search_query"] + "\nRESULTS: " + execResult;

            Console.WriteLine("📚 Found information, analyzing results...");

            // Always go back to the decision node after searching
            return "decide";
        }
    }
}
