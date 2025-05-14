using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Search_Agent
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
            //// Search the web for the given query
            //Console.WriteLine($"🌐 Searching the web for: {searchQuery}");
            //var results = await Utils.SearchWeb(searchQuery.ToString());
            //return results;    
            Console.WriteLine($"🌐 Searching the web for: {searchQuery}");
            string query = searchQuery.ToString();
            var results = Utils.SearchWebSync(query);
            return results;
        }

        public override string Post(Dictionary<string, object> shared, object prepResult, object execResult)
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
