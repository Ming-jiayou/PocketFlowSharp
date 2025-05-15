using OpenAI.Chat;
using OpenAI;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.AI;
using ChatMessage = OpenAI.Chat.ChatMessage;
using System.ClientModel;

namespace Summarize
{
    public static class Utils
    {
        public static string ModelName { get; set; }
        public static string EndPoint { get; set; }
        public static string ApiKey { get; set; }

        public static string CallLLM(string prompt)
        {
            var messages = new List<ChatMessage>
            {
                new UserChatMessage(prompt)
            };

            ApiKeyCredential apiKeyCredential = new ApiKeyCredential(ApiKey);

            OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions();
            openAIClientOptions.Endpoint = new Uri(EndPoint);

            ChatClient client = new(model: ModelName, apiKeyCredential, openAIClientOptions);
          
            ChatCompletion completion = client.CompleteChat(messages);

            return completion.Content[0].Text;
        }
    }
} 