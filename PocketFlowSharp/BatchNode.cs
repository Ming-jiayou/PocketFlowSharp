using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketFlowSharp
{
    public class BatchNode : Node
    {
        public BatchNode(int maxRetries = 1, int waitMilliseconds = 0) 
            : base(maxRetries, waitMilliseconds)
        {
        }

        public override object Exec(object prepResult)
        {
            if (prepResult == null)
            {
                return new List<object>();
            }

            if (prepResult is IEnumerable<object> items)
            {
                return items.Select(item => base.Exec(item)).ToList();
            }

            throw new ArgumentException("BatchNode expects an IEnumerable<object> as input");
        }
    }
} 