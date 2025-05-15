using OpenAI.Chat;
using OpenAI;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.AI;
using ChatMessage = OpenAI.Chat.ChatMessage;
using System.ClientModel;

namespace Chat_Guardrail
{
    public static class Utils
    {
        public static string ModelName { get; set; }
        public static string EndPoint { get; set; }
        public static string ApiKey { get; set; }

        /// <summary>
        /// 调用LLM模型获取回复
        /// </summary>
        public static string CallLLM(List<ChatMessage> messages)
        {
            ApiKeyCredential apiKeyCredential = new ApiKeyCredential(ApiKey);

            OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions();
            openAIClientOptions.Endpoint = new Uri(EndPoint);

            ChatClient client = new(model: ModelName, apiKeyCredential, openAIClientOptions);
            
            ChatCompletion completion = client.CompleteChat(messages);

            return completion.Content[0].Text;
        }

        /// <summary>
        /// 使用YAML格式解析LLM响应
        /// </summary>
        public static (bool isValid, string reason) ParseYamlResponse(string response)
        {
            try
            {
                // 简单的YAML解析（实际项目中应使用专门的YAML解析库）
                string[] lines = response.Split('\n');
                bool valid = false;
                string reason = "";

                foreach (var line in lines)
                {
                    if (line.StartsWith("valid:"))
                    {
                        valid = line.ToLower().Contains("true");
                    }
                    else if (line.StartsWith("reason:"))
                    {
                        reason = line.Substring("reason:".Length).Trim();
                    }
                }

                return (valid, reason);
            }
            catch
            {
                return (false, "无法解析响应格式");
            }
        }
    }
} 