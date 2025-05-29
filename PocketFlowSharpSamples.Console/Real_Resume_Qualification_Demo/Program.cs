using dotenv.net;
using PocketFlowSharp;
using System;
using System.IO;

namespace Real_Resume_Qualification_Demo
{
    internal class Program
    {
        static void Main(string[] args)
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

            // 创建简历处理流程
            Flow flow = CreateResumeFlow();

            // 初始化共享存储
            var shared = new Dictionary<string, object>();
            shared["requirements"] = """
                - 具备后端开发能力
                - 熟悉go语言
                """;

            // 运行流程
            Console.WriteLine("开始简历资格评估处理...");
            flow.Run(shared);

            // 显示最终汇总信息（除了在ReduceResultsNode中已经打印的内容）
            if (shared.ContainsKey("summary"))
            {
                Console.WriteLine("\n详细评估结果：");
                var evaluations = shared["evaluations"] as Dictionary<string, Dictionary<string, object>>;
                if (evaluations != null)
                {
                    foreach (var (filename, evaluation) in evaluations)
                    {
                        var qualified = (bool)evaluation["qualifies"] ? "✓" : "✗";
                        var name = evaluation["candidate_name"]?.ToString() ?? "未知";
                        Console.WriteLine($"{qualified} {name} ({filename})");
                    }
                }
            }

            Console.WriteLine("\n简历处理完成！");
        }

        public static Flow CreateResumeFlow()
        {
            // 创建节点实例
            var readResumes = new ReadResumesNode();
            var evaluateResumes = new EvaluateResumesNode();
            var reduceResults = new ReduceResultsNode();

            // 连接节点
            _ = readResumes - "default" - evaluateResumes - "default" - reduceResults;

            // 创建流程
            return new Flow(readResumes);
        }
    }
}
