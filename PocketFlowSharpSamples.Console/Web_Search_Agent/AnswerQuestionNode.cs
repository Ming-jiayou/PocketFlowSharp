using OpenAI.Chat;
using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web_Search_Agent
{
    public class AnswerQuestionNode : AsyncNode
    {
        private CancellationTokenSource _cts;
        private Task _inputListenerTask;

        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            // 创建取消令牌源
            _cts = new CancellationTokenSource();

            // 启动一个任务来监听用户输入
            Console.WriteLine("按回车键随时中断流式输出...");
            _inputListenerTask = Task.Run(() =>
            {
                Console.ReadLine();
                _cts.Cancel();
            });

            // Get the question and context for answering
            string question = (string)shared["question"];
            string context = shared.ContainsKey("context") ? (string)shared["context"] : "";
            return new Tuple<string, string>(question, context);
        }

        protected override async Task<object> ExecAsync(object inputs)
        {
            // Call the LLM to generate a final answer
            var (question, context) = (Tuple<string, string>)inputs;

            Console.WriteLine("✏️ Crafting final answer...");

            // Create a prompt for the LLM to answer the question
            string prompt = $@"
### CONTEXT
Based on the following information, answer the question.
Question: {question}
Research: {context}

## YOUR ANSWER:
Provide a comprehensive answer using the research results.
";

            try
            {
                var result = await Utils.CallLLMStreamingAsync(prompt, _cts.Token);

                Console.ForegroundColor = ConsoleColor.Green;
                StringBuilder answerBuilder = new StringBuilder();
                
                await foreach (StreamingChatCompletionUpdate completionUpdate in result)
                {
                    if (_cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("\n用户中断了流式输出。");
                        break;
                    }

                    if (completionUpdate.ContentUpdate.Count > 0)
                    {
                        string text = completionUpdate.ContentUpdate[0].Text;
                        Console.Write(text);
                        answerBuilder.Append(text);
                        Task.Delay(50).Wait(); // 控制输出速度
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                return answerBuilder.ToString();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n流式输出已被取消。");
                return "答案生成被用户中断";
            }
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // Save the final answer and complete the flow
            shared["answer"] = execResult;

            Console.WriteLine("\n✅ Answer generated successfully");

            // 清理资源
            _cts.Cancel();
            await _inputListenerTask;
            _cts.Dispose();

            // We're done - no need to continue the flow
            return "done";
        }
    }
}
