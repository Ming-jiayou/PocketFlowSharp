using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PocketFlowSharp;

namespace Async_Basic
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Create shared data dictionary
            var shared = new Dictionary<string, object>();

            // Create and run flow
            var recipeFlow = CreateFlow();
            Console.WriteLine("\nWelcome to Recipe Finder!");
            Console.WriteLine("------------------------");
            await recipeFlow.RunAsync(shared);
            Console.WriteLine("\nThanks for using Recipe Finder!");
        }

        static AsyncFlow CreateFlow()
        {
            // Create node instances
            var fetchNode = new FetchRecipesNode();
            var suggestNode = new SuggestRecipeNode();
            var approvalNode = new GetApprovalNode();
            var endNode = new NoOpNode();

            // Create flow starting with fetch node
            var flow = new AsyncFlow(fetchNode);

            // Connect nodes
            _ = fetchNode - "suggest" - suggestNode;
            _ = suggestNode - "approve" - approvalNode;
            _ = approvalNode - "retry" - suggestNode;    // Loop back for another suggestion
            _ = approvalNode - "accept" - endNode;       // Properly end the flow

            return flow;
        }
    }
}
