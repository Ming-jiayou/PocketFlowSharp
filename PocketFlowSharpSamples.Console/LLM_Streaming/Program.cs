using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotenv.net;
using PocketFlowSharp;

namespace LLM_Streaming
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Load .env file
            DotEnv.Load();

            // Get environment variables from .env file
            var envVars = DotEnv.Read();

            string ModelName = envVars["ModelName"];
            string EndPoint = envVars["EndPoint"];
            string ApiKey = envVars["ApiKey"];

            Utils.ModelName = ModelName;
            Utils.EndPoint = EndPoint;
            Utils.ApiKey = ApiKey;

            // 创建共享数据字典
            var shared = new Dictionary<string, object>
            {
                { "prompt", "写一首关于春天的诗歌" }
            };

            // 创建并运行流程
            var streamingFlow = CreateFlow();
            Console.WriteLine("\n欢迎使用LLM流式输出示例！");
            Console.WriteLine("------------------------");
            await streamingFlow.RunAsync(shared);
            Console.WriteLine("\n感谢使用LLM流式输出示例！");
        }

        static AsyncFlow CreateFlow()
        {
            // 创建节点实例
            var streamNode = new StreamNode();           

            // 创建从流式输出节点开始的流程
            var flow = new AsyncFlow(streamNode);

            return flow;
        }
    }
}
