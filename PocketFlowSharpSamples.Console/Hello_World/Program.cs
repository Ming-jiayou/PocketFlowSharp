using System;
using System.Collections.Generic;
using dotenv.net;
using PocketFlowSharp;

namespace Hello_World
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load .env file
            DotEnv.Load();

            // Get environment variables from .env file
            var envVars = DotEnv.Read();

            string ModelName = envVars["ModelName"];
            string EndPoint = envVars["EndPoint"];
            string ApiKey = envVars["ApiKey"];         

            Utils.ModelName = ModelName;
            Utils.EndPoint = EndPoint;
            Utils.ApiKey = ApiKey;
            
            // Create a dictionary to share data between nodes
            var shared = new Dictionary<string, object>
            {
                { "question", "你是谁？" },
                { "answer", null }
            };

            // Create the flow
            var qaFlow = CreateFlow();
            
            // Run the flow with shared data
            qaFlow.Run(shared);
            
            // Display results
            Console.WriteLine("Question: " + shared["question"]);
            Console.WriteLine("Answer: " + shared["answer"]);
        }

        static Flow CreateFlow()
        {
            // Create an instance of AnswerNode
            var answerNode = new AnswerNode();
            
            // Create a flow starting with the answer node
            var flow = new Flow(answerNode);
            
            return flow;
        }
    }
}
