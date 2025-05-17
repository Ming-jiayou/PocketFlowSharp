using OpenAI.Chat;
using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LLM_Streaming
{
    /// <summary>
    /// 流式输出节点
    /// </summary>
    public class StreamNode : AsyncNode
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

            // 从共享存储中获取提示词
            string prompt = (string)shared["prompt"];
            return prompt;
        }

        protected override async Task<object> ExecAsync(object prepResult)
        {
            string prompt = (string)prepResult;
            
            try
            {
                var result = await Utils.CallLLMStreamingAsync(prompt, _cts.Token);

                Console.ForegroundColor = ConsoleColor.Green;
                await foreach (StreamingChatCompletionUpdate completionUpdate in result)
                {
                    if (_cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("\n用户中断了流式输出。");
                        break;
                    }

                    if (completionUpdate.ContentUpdate.Count > 0)
                    {
                        Console.Write(completionUpdate.ContentUpdate[0].Text);
                        Task.Delay(100).Wait(); // 控制输出速度
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n流式输出已被取消。");
            }

            return null;
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // 清理资源
            _cts.Cancel();
            await _inputListenerTask;
            _cts.Dispose();
            
            return "default";
        }
    }  
} 