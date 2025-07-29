using PocketFlowSharp;
using PocketFlowSharpGallery.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Models.WebSearchAgent
{
    /// <summary>
    /// 异步决策节点，决定是搜索还是直接回答
    /// </summary>
    public class DecideActionAsyncNode : AsyncNode
    {
        private readonly IProgressReporter _progressReporter;

        public DecideActionAsyncNode(IProgressReporter progressReporter)
        {
            _progressReporter = progressReporter ?? throw new ArgumentNullException(nameof(progressReporter));
        }

        protected override async Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            // 准备决策所需的上下文和问题
            string context = shared.ContainsKey("context") ? (string)shared["context"] : "No previous search";
            string question = (string)shared["question"];

            _progressReporter.ReportProgress("decision", "Analyzing question...", 10);
            _progressReporter.ReportIntermediateResult("decision", $"Analyzing: {question}");

            return new Tuple<string, string>(question, context);
        }

        protected override async Task<object> ExecAsync(object inputs)
        {
            var (question, context) = (Tuple<string, string>)inputs;

            _progressReporter.ReportProgress("decision", "Consulting AI for decision...", 20);
            _progressReporter.ReportIntermediateResult("decision", "AI is deciding whether to search or answer directly...");

            // 创建提示词帮助LLM决定下一步行动
            string prompt = $@"
### CONTEXT
You are a research assistant that can search the web.
Question: {question}
Previous Research: {context}

### ACTION SPACE
[1] search
  Description: Look up more information on the web
  Parameters:
    - query (str): What to search for

[2] answer
  Description: Answer the question with current knowledge
  Parameters:
    - answer (str): Final answer to the question

## NEXT ACTION
Decide the next action based on the context and available actions.
Return your response in this format:

```yaml
thinking: |
    <your step-by-step reasoning process>
action: search OR answer
reason: <why you chose this action>
answer: <if action is answer>
search_query: <specific search query if action is search>
```
IMPORTANT: Make sure to:
1. Use proper indentation (4 spaces) for all multi-line fields
2. Use the | character for multi-line text fields
3. Keep single-line fields without the | character
";

            try
            {
                // 调用LLM进行决策
                string response = await Utils.CallLLMAsync(prompt);
                
                // 解析响应获取决策
                string yamlStr = response.Split("```yaml")[1].Split("```")[0].Trim();
                var decision = Utils.ParseSimpleYaml(yamlStr);

                _progressReporter.ReportIntermediateResult("decision", 
                    $"AI decided to: {decision["action"]} (Reason: {decision["reason"]})");

                return decision;
            }
            catch (Exception ex)
            {
                _progressReporter.ReportError($"Failed to parse LLM response: {ex.Message}");
                
                // 回退决策
                return new Dictionary<string, object>
                {
                    { "action", "answer" },
                    { "reason", "Failed to parse LLM response" },
                    { "answer", "Sorry, I encountered an error processing your request." }
                };
            }
        }

        protected override async Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            var decision = (Dictionary<string, object>)execResult;

            if (decision["action"].ToString() == "search")
            {
                shared["search_query"] = decision["search_query"];
                _progressReporter.ReportProgress("decision", $"Decision made: Search for '{decision["search_query"]}'", 30);
                _progressReporter.ReportIntermediateResult("search", $"Preparing to search: {decision["search_query"]}");
            }
            else
            {
                shared["context"] = decision["answer"];
                _progressReporter.ReportProgress("decision", "Decision made: Answer directly", 30);
                _progressReporter.ReportIntermediateResult("answer", "Preparing final answer...");
            }

            return decision["action"]?.ToString() ?? "answer";
        }
    }
}