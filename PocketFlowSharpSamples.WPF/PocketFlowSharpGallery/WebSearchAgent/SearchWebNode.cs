using PocketFlowSharp;

namespace PocketFlowSharpGallery.WebSearchAgent
{
    public class SearchWebNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // Get the search query from the shared store
            return shared["search_query"];
        }

        public override object Exec(object searchQuery)
        {
            string query = searchQuery.ToString() ?? "";
            var results = Utils.SearchWebSync(query);
            return results;
        }

        public override string Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // Save the search results and go back to the decision node
            string previous = shared.ContainsKey("context") ? (string)shared["context"] : "";
            shared["context"] = previous + "\n\nSEARCH: " + shared["search_query"] + "\nRESULTS: " + execResult;

            // Always go back to the decision node after searching
            return "decide";
        }
    }
} 