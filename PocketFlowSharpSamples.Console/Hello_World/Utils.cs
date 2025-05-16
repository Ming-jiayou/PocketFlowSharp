using OpenAI.Chat;
using OpenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello_World
{
    public static class Utils
    {
        public static string ModelName { get; set; }
        public static string EndPoint { get; set; }
        public static string ApiKey { get; set; }

        public static string CallLLM(string prompt)
        {
            ApiKeyCredential apiKeyCredential = new ApiKeyCredential(ApiKey);

            OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions();
            openAIClientOptions.Endpoint = new Uri(EndPoint);

            ChatClient client = new(model: ModelName, apiKeyCredential, openAIClientOptions);

            ChatCompletion completion = client.CompleteChat(prompt);

            return completion.Content[0].Text;
        }
    }
} 