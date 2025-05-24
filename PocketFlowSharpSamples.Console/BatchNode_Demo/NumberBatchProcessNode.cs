using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchNode_Demo
{
    public class NumberBatchProcessNode : BatchNode
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // 获取要处理的数字列表
            var numbers = (List<int>)shared["numbers"];
            return numbers.Cast<object>().ToList();
        }

        public override object Exec(object prepResult)
        {
            // 对单个数字进行处理（这里我们简单地将数字乘以2）
            if (prepResult is int number)
            {
                Console.WriteLine($"处理数字 {number}");
                return number * 2;
            }
            return base.Exec(prepResult);
        }

        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            shared["results"] = execResult;

            return null;
        }
    }
}
