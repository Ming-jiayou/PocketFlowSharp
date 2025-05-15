using System;
using System.Collections.Generic;
using dotenv.net;
using PocketFlowSharp;

namespace Chat_Guardrail
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 加载.env文件
            DotEnv.Load();

            // 从.env文件获取环境变量
            var envVars = DotEnv.Read();

            // 设置LLM配置
            Utils.ModelName = envVars["ModelName"];
            Utils.EndPoint = envVars["EndPoint"];
            Utils.ApiKey = envVars["ApiKey"];

            // 创建共享数据字典
            var shared = new Dictionary<string, object>();

            // 创建并运行流程
            var chatFlow = CreateFlow();
            chatFlow.Run(shared);
        }

        static Flow CreateFlow()
        {
            // 创建节点实例
            var userInputNode = new UserInputNode();
            var guardrailNode = new GuardrailNode();
            var llmNode = new LLMNode();

            // 创建以用户输入节点开始的流程
            var flow = new Flow(userInputNode);

            // 添加节点连接
            //userInputNode.Next(guardrailNode, "validate");  // 用户输入 -> 验证
            //guardrailNode.Next(userInputNode, "retry");     // 验证失败 -> 重新输入
            //guardrailNode.Next(llmNode, "process");         // 验证成功 -> LLM处理
            //llmNode.Next(userInputNode, "continue");        // 继续对话
            _ = userInputNode - "validate" - guardrailNode;
            _ = guardrailNode - "retry" - userInputNode;
            _ = guardrailNode - "process" - llmNode;
            _ = llmNode - "continue" - userInputNode;

            return flow;
        }
    }
}
