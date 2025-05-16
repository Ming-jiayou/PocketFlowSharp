using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Async_Basic
{
    /// <summary>
    /// Node that fetches recipes based on an ingredient
    /// </summary>
    public class FetchRecipesNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            Console.Write("Enter ingredient: ");
            string ingredient = Console.ReadLine();
            return ingredient;
        }

        protected override async Task<object> ExecAsync(object prepResult)
        {
            string ingredient = (string)prepResult;
            Console.WriteLine($"Fetching recipes for {ingredient}...");

            // Simulate API delay
            await Task.Delay(1000);

            // Mock recipes (in real app, would fetch from API)
            var recipes = new List<string>
            {
                $"{ingredient} Stir Fry",
                $"Grilled {ingredient} with Herbs",
                $"Baked {ingredient} with Vegetables"
            };

            Console.WriteLine($"Found {recipes.Count} recipes.");
            return recipes;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string ingredient = (string)prepResult;
            var recipes = (List<string>)execResult;

            shared["recipes"] = recipes;
            shared["ingredient"] = ingredient;

            return "suggest";
        }
    }

    /// <summary>
    /// Node that suggests a recipe using LLM
    /// </summary>
    public class SuggestRecipeNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            return shared["recipes"];
        }

        protected override async Task<object> ExecAsync(object prepResult)
        {
            var recipes = (List<string>)prepResult;
            Console.WriteLine("\nSuggesting best recipe...");

            // Simulate LLM delay
            await Task.Delay(1000);

            // Mock LLM suggestion (in real app, would use LLM)
            string suggestion = recipes[1];  // Always suggest second recipe
            Console.WriteLine($"How about: {suggestion}");

            return suggestion;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string suggestion = (string)execResult;
            shared["suggestion"] = suggestion;
            return "approve";
        }
    }

    /// <summary>
    /// Node that gets user approval for the suggested recipe
    /// </summary>
    public class GetApprovalNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            return shared["suggestion"];
        }

        protected override async Task<object> ExecAsync(object prepResult)
        {
            string suggestion = (string)prepResult;
            Console.Write($"\nAccept this recipe? (y/n): ");
            string answer = Console.ReadLine()?.ToLower() ?? "n";
            return answer;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string answer = (string)execResult;

            if (answer == "y")
            {
                Console.WriteLine("\nGreat choice! Here's your recipe...");
                Console.WriteLine($"Recipe: {shared["suggestion"]}");
                Console.WriteLine($"Ingredient: {shared["ingredient"]}");
                return "accept";
            }
            else
            {
                Console.WriteLine("\nLet's try another recipe...");
                return "retry";
            }
        }
    }

    /// <summary>
    /// Node that does nothing, used to properly end the flow
    /// </summary>
    public class NoOpNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared) => null;
        protected override async Task<object> ExecAsync(object prepResult) => null;
        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult) => null;
    }
} 