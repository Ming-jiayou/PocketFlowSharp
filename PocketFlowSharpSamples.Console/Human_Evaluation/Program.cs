using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotenv.net;
using PocketFlowSharp;

namespace Human_Evaluation
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
            var shared = new Dictionary<string, object>();

            // 创建并运行流程
            var humanEvalFlow = CreateFlow();
            Console.WriteLine("\n欢迎使用人工判断示例！");
            Console.WriteLine("------------------------");
            await humanEvalFlow.RunAsync(shared);
            Console.WriteLine("\n感谢使用人工判断示例！");
        }

        static AsyncFlow CreateFlow()
        {
            // 创建节点实例
            var inputNode = new TaskInputNode();
            var aiResponseNode = new AIResponseNode();
            var humanApprovalNode = new HumanApprovalNode();
            var endNode = new NoOpNode();

            // 创建从输入节点开始的流程
            var flow = new AsyncFlow(inputNode);

            // 连接节点
            _ = inputNode - "generate" - aiResponseNode;
            _ = aiResponseNode - "approve" - humanApprovalNode;
            _ = humanApprovalNode - "retry" - aiResponseNode;     // 不接受时重新生成
            _ = humanApprovalNode - "accept" - endNode;          // 接受时结束流程

            return flow;
        }
    }
}
