using System;
using System.Collections.Generic;
using dotenv.net;
using PocketFlowSharp;

namespace Chat
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
            var shared = new Dictionary<string, object>();

            // Create the flow
            var chatFlow = CreateFlow();
            
            // Run the flow with shared data
            chatFlow.Run(shared);
        }

        static Flow CreateFlow()
        {
            // Create an instance of ChatNode
            var chatNode = new ChatNode();
            
            // Create a flow starting with the chat node
            var flow = new Flow(chatNode);

            // Add self-loop to continue the conversation
            // _ = chatNode - "continue" - chatNode;
            chatNode.Next(chatNode, "continue");

            return flow;
        }
    }
}
