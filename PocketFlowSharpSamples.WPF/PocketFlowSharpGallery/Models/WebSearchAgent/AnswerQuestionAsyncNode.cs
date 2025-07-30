using PocketFlowSharp;
using PocketFlowSharpGallery.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Models.WebSearchAgent
{
    /// <summary>
    /// 异步回答节点，生成最终答案
    /// </summary>
    public class AnswerQuestionAsyncNode : AsyncNode
    {
        private readonly IProgressReporter _progressReporter;

        public AnswerQuestionAsyncNode(IProgressReporter progressReporter)
        {
            _progressReporter = progressReporter ?? throw new ArgumentNullException(nameof(progressReporter));
        }

        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            // 获取问题和上下文
            string question = (string)shared["question"];
            string context = shared.ContainsKey("context") ? (string)shared["context"] : "";

            return new Tuple<string, string>(question, context);
        }

        protected override async Task<object> ExecAsync(object inputs)
        {
            var (question, context) = (Tuple<string, string>)inputs;

            _progressReporter.PrintMessage("[回答节点] 咨询AI生成最终答案...");

            // 创建提示词生成最终答案
            string prompt = $@"
### CONTEXT
Based on the following information, provide a comprehensive answer to the question.

Question: {question}

Research Results:
{context}

## YOUR ANSWER:
Provide a detailed, accurate, and helpful answer using the research results above. Be concise but thorough.
";

            try
            {
                // 使用异步调用获取答案
                string answer = await Utils.CallLLMAsync(prompt);
                
                _progressReporter.PrintMessage("[回答节点] 答案生成成功");

                return answer;
            }
            catch (OperationCanceledException)
            {
                _progressReporter.PrintMessage("[回答节点] 答案生成已取消");
                return "Answer generation was cancelled by user";
            }
            catch (Exception ex)
            {
                _progressReporter.PrintMessage($"[回答节点] 错误: 生成答案失败 - {ex.Message}");
                throw;
            }
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // 保存最终答案
            string answer = execResult.ToString() ?? "No answer generated";
            shared["answer"] = answer;

            return "done";
        }
    }
}