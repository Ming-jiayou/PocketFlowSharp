using System;
using System.Collections.Generic;
using System.Linq;
using PocketFlowSharp;

namespace BatchNode_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建要处理的数字列表
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            Console.WriteLine("原始数字：" + string.Join(", ", numbers));

            var shared = new Dictionary<string, object>
            {
                { "numbers", numbers }
            };

            var batchFlow = CreateFlow();
            batchFlow.Run(shared);

            var results = (List<object>)shared["results"];

            Console.WriteLine("处理后的数字：" + string.Join(", ", (List<object>)results));
        }

        static Flow CreateFlow()
        {
            var numberBatchNode = new NumberBatchProcessNode();
            var flow = new Flow(numberBatchNode);
            return flow;
        }
    } 
}
