using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LLM_Streaming
{
    public static class Utils
    {
        public static string ModelName { get; set; }
        public static string EndPoint { get; set; }
        public static string ApiKey { get; set; }
           
        public static async Task<AsyncCollectionResult<StreamingChatCompletionUpdate>> CallLLMStreamingAsync(string prompt, CancellationToken cancellationToken = default)
        {
            ApiKeyCredential apiKeyCredential = new ApiKeyCredential(ApiKey);

            OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions();
            openAIClientOptions.Endpoint = new Uri(EndPoint);

            ChatClient client = new(model: ModelName, apiKeyCredential, openAIClientOptions);

            ChatCompletionOptions chatCompletionOptions = new ChatCompletionOptions();

            List<ChatMessage> messages = new List<ChatMessage>();
            messages.Add(new UserChatMessage(prompt));

            var completion = client.CompleteChatStreamingAsync(messages, chatCompletionOptions,cancellationToken);

            return completion;
        }
    }
} 