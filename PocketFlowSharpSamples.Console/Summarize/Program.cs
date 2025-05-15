using System;
using System.Collections.Generic;
using dotenv.net;
using PocketFlowSharp;

namespace Summarize
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 加载 .env 文件
            DotEnv.Load();

            // 从 .env 文件获取环境变量
            var envVars = DotEnv.Read();

            string ModelName = envVars["ModelName"];
            string EndPoint = envVars["EndPoint"];
            string ApiKey = envVars["ApiKey"];         

            Utils.ModelName = ModelName;
            Utils.EndPoint = EndPoint;
            Utils.ApiKey = ApiKey;
            
            // 创建共享数据字典
            var shared = new Dictionary<string, object>();

            // 示例文本
            string text = @"
                PocketFlow is a minimalist LLM framework that models workflows as a Nested Directed Graph.
                Nodes handle simple LLM tasks, connecting through Actions for Agents.
                Flows orchestrate these nodes for Task Decomposition, and can be nested.
                It also supports Batch processing and Async execution.
            ";

            // 将文本存入共享数据
            shared["data"] = text;

            // 创建流程
            var summarizeFlow = CreateFlow();
            
            // 运行流程
            summarizeFlow.Run(shared);

            // 输出结果
            Console.WriteLine("\n输入文本:");
            Console.WriteLine(text);
            Console.WriteLine("\n摘要:");
            Console.WriteLine(shared["summary"]);
        }

        static Flow CreateFlow()
        {
            // 创建摘要节点实例
            var summarizeNode = new SummarizeNode();
            
            // 创建以摘要节点为起点的流程
            var flow = new Flow(summarizeNode);

            return flow;
        }
    }
}
