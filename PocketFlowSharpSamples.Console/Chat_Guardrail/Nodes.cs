using PocketFlowSharp;
using System;
using System.Collections.Generic;
using OpenAI.Chat;

namespace Chat_Guardrail
{
    /// <summary>
    /// 用户输入节点：处理用户输入并进行初始化
    /// </summary>
    public class UserInputNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // 首次运行时初始化消息列表
            if (!shared.ContainsKey("messages"))
            {
                shared["messages"] = new List<ChatMessage>();
                Console.WriteLine("欢迎使用旅行顾问聊天！输入'exit'结束对话。");
            }
            
            return null;
        }

        public override object Exec(object prepResult)
        {
            // 获取用户输入
            Console.Write("\n您：");
            string userInput = Console.ReadLine();
            return userInput;
        }

        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string userInput = (string)execResult;

            // 检查用户是否要退出
            if (userInput?.ToLower() == "exit")
            {
                Console.WriteLine("\n再见！祝您旅途愉快！");
                return null;  // 结束对话
            }

            // 将用户输入存储在共享数据中
            shared["user_input"] = userInput;

            // 进入验证环节
            return "validate";
        }
    }

    /// <summary>
    /// 验证节点：确保用户输入是旅行相关的问题
    /// </summary>
    public class GuardrailNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // 从共享数据中获取用户输入
            return shared.GetValueOrDefault("user_input", "");
        }

        public override object Exec(object prepResult)
        {
            string userInput = (string)prepResult;

            // 基本验证检查
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return (false, "您的问题是空的，请提供一个旅行相关的问题。");
            }

            if (userInput.Trim().Length < 3)
            {
                return (false, "您的问题太短了，请提供更多关于您的旅行问题的细节。");
            }

            // 使用LLM进行旅行主题验证
            string prompt = $@"
评估以下用户查询是否与旅行建议、目的地、规划或其他旅行主题相关。
聊天应该只回答与旅行相关的问题，并拒绝任何离题、有害或不当的查询。
用户查询: {userInput}
请用YAML格式返回您的评估:
```yaml
valid: true/false
reason: [解释为什么查询有效或无效]
```";

            var messages = new List<ChatMessage>
            {
                new UserChatMessage(prompt)
            };

            string response = Utils.CallLLM(messages);
            var (isValid, reason) = Utils.ParseYamlResponse(response);

            return (isValid, reason);
        }

        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            var (isValid, message) = ((bool, string))execResult;

            if (!isValid)
            {
                // 向用户显示错误消息
                Console.WriteLine($"\n旅行顾问：{message}");
                // 跳过LLM调用，返回用户输入
                return "retry";
            }

            // 有效输入，添加到消息历史
            var messages = (List<ChatMessage>)shared["messages"];
            messages.Add(new UserChatMessage((string)shared["user_input"]));
            
            // 继续处理LLM
            return "process";
        }
    }

    /// <summary>
    /// LLM节点：处理与语言模型的交互
    /// </summary>
    public class LLMNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            var messages = (List<ChatMessage>)shared["messages"];

            // 如果没有系统消息，添加一个
            if (!messages.Exists(msg => msg is SystemChatMessage))
            {
                messages.Insert(0, new SystemChatMessage(
                    "您是一位乐于助人的旅行顾问，提供有关目的地、旅行规划、住宿、交通、活动和其他旅行相关主题的信息。" +
                    "只回答与旅行相关的查询，保持回答信息丰富友好。您的回答简明扼要，控制在100字以内。"));
            }

            return messages;
        }

        public override object Exec(object prepResult)
        {
            var messages = (List<ChatMessage>)prepResult;
            return Utils.CallLLM(messages);
        }

        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string response = (string)execResult;
            
            // 打印助手的回复
            Console.WriteLine($"\n旅行顾问：{response}");

            // 将助手消息添加到历史记录
            var messages = (List<ChatMessage>)shared["messages"];
            messages.Add(new AssistantChatMessage(response));

            // 继续对话
            return "continue";
        }
    }
} 