using System;
using System.Collections.Generic;
using PocketFlowSharp;

namespace Summarize
{
    public class SummarizeNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // 从共享存储中读取并预处理数据
            if (shared.ContainsKey("data"))
            {
                return shared["data"];
            }
            return null;
        }

        public override object Exec(object prepRes)
        {
            // 使用LLM执行摘要
            if (prepRes == null)
            {
                return "空文本";
            }

            try
            {
                string prompt = $"用10个字总结这段文本: {prepRes}";
                string summary = Utils.CallLLM(prompt);
                return summary;
            }
            catch (Exception)
            {
                return ExecFallback(prepRes, null);
            }
        }

        public override object ExecFallback(object prepResult, Exception exception)
        {
            // 提供简单的失败回退而不是崩溃
            return "处理您的请求时出现错误。";
        }

        public override string Post(Dictionary<string, object> shared, object prepRes, object execRes)
        {
            // 将摘要存储在共享存储中
            shared["summary"] = execRes;
            return "default";
        }
    }
} 