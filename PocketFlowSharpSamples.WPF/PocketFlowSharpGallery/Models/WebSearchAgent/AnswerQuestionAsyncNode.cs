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

            _progressReporter.ReportProgress("answer", "Preparing final answer...", 85);
            _progressReporter.ReportIntermediateResult("answer", "Generating comprehensive answer...");

            return new Tuple<string, string>(question, context);
        }

        protected override async Task<object> ExecAsync(object inputs)
        {
            var (question, context) = (Tuple<string, string>)inputs;

            _progressReporter.ReportProgress("answer", "Consulting AI for final answer...", 90);
            _progressReporter.ReportIntermediateResult("answer", "AI is crafting the final response...");

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
                
                _progressReporter.ReportProgress("answer", "Finalizing answer...", 95);
                _progressReporter.ReportIntermediateResult("answer", "Answer generated successfully");

                return answer;
            }
            catch (OperationCanceledException)
            {
                _progressReporter.ReportIntermediateResult("info", "Answer generation was cancelled");
                return "Answer generation was cancelled by user";
            }
            catch (Exception ex)
            {
                _progressReporter.ReportError($"Failed to generate answer: {ex.Message}");
                throw;
            }
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // 保存最终答案
            string answer = execResult.ToString() ?? "No answer generated";
            shared["answer"] = answer;

            _progressReporter.ReportProgress("answer", "Answer complete", 100);
            _progressReporter.ReportIntermediateResult("answer", "Process completed successfully");

            return "done";
        }
    }
}