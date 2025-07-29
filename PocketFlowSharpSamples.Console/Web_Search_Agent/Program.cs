using System;
using dotenv.net;
using PocketFlowSharp;

namespace Web_Search_Agent
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Load .env file
            DotEnv.Load();

            // Get environment variables from .env file
            var envVars = DotEnv.Read();
            
            string ModelName = envVars["ModelName"];
            string EndPoint = envVars["EndPoint"];
            string ApiKey = envVars["ApiKey"];
            string BraveSearchApiKey = envVars["BraveSearchApiKey"];

            Utils.ModelName = ModelName;
            Utils.EndPoint = EndPoint;
            Utils.ApiKey = ApiKey;
            Utils.BraveSearchApiKey = BraveSearchApiKey;
           
            // Get question from command line if provided
            string question = "智谱2025年最新模型是什么？";

            // Create the agent flow
            AsyncFlow flow = CreatFlow();

            // Process the question
            var shared = new Dictionary<string, object> { { "question", question } };
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"🤔 Processing question: {question}");
            await flow.RunAsync(shared);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n🎯 Final Answer:");
            Console.WriteLine(shared.ContainsKey("answer") ? shared["answer"] : "No answer found");

        }

        public static AsyncFlow CreatFlow()
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
            // search.Next(decide, "decide");
            _ = search - "decide" - decide;

            // Create the flow, starting with the DecideAction node
            var flow = new AsyncFlow(decide);

            return flow;
        }
    }
}
