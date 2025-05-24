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
            // Handle null case
            if (prepResult == null)
            {
                return new List<object>();
            }

            // If the input is already a list, process each item
            if (prepResult is IEnumerable<object> items)
            {
                return items.Select(item => ProcessSingleItem(item)).ToList();
            }

            // If it's a single item, process it directly
            return ProcessSingleItem(prepResult);
        }

        private object ProcessSingleItem(object item)
        {
            return ((Node)this).Exec(item);
        }
    }
} 