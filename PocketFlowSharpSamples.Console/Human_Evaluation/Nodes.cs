using OpenAI.Chat;
using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Human_Evaluation
{
    /// <summary>
    /// 任务输入节点
    /// </summary>
    public class TaskInputNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            Console.WriteLine("\n请输入需要AI处理的任务：");
            string task = Console.ReadLine();
            return task;
        }

        protected override async Task<object> ExecAsync(object prepResult)
        {
            string task = (string)prepResult;
            Console.WriteLine($"\n已收到任务：{task}");
            return task;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string task = (string)execResult;
            shared["task"] = task;
            return "generate";
        }
    }

    /// <summary>
    /// AI响应节点
    /// </summary>
    public class AIResponseNode : AsyncNode
    {
        private static int attemptCount = 0;

        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            return shared["task"];
        }

        protected override async Task<object> ExecAsync(object prepResult)
        {
            string task = (string)prepResult;
            attemptCount++;
            
            Console.WriteLine("AI正在生成回复...\n");
            Console.WriteLine($"任务：{task}\n");
            Console.WriteLine($"这是第 {attemptCount} 次生成的AI回复：\n");
            var result = await Utils.CallLLMStreamingAsync(task);

            string response="";
            Console.ForegroundColor = ConsoleColor.Green;
            await foreach (StreamingChatCompletionUpdate completionUpdate in result)
            {
                if (completionUpdate.ContentUpdate.Count > 0)
                {
                    Console.Write(completionUpdate.ContentUpdate[0].Text);
                    response += completionUpdate.ContentUpdate[0].Text.ToString();
                }
            }
            Console.ForegroundColor = ConsoleColor.White;

            return response;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string response = (string)execResult;
            shared["response"] = response;
            return "approve";
        }
    }

    /// <summary>
    /// 人工审批节点
    /// </summary>
    public class HumanApprovalNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            return shared["response"];
        }

        protected override async Task<object> ExecAsync(object prepResult)
        {
            Console.Write("\n您接受这个AI回复吗？(y/n): ");
            string answer = Console.ReadLine()?.ToLower() ?? "n";
            return answer;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            string answer = (string)execResult;

            if (answer == "y")
            {
                Console.WriteLine($"已接受的回复：\n{shared["response"]}");
                return "accept";
            }
            else
            {
                Console.WriteLine("\n好的，让AI重新生成回复...");
                return "retry";
            }
        }
    }

    /// <summary>
    /// 空操作节点，用于正确结束流程
    /// </summary>
    public class NoOpNode : AsyncNode
    {
        protected override async Task<object> PrepAsync(Dictionary<string, object> shared) => null;
        protected override async Task<object> ExecAsync(object prepResult) => null;
        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult) => null;
    }
} 