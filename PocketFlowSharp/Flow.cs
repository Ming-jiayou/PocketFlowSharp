using System;
using System.Collections.Generic;
using System.Text;

namespace PocketFlowSharp
{
    public class Flow : BaseNode
    {
        protected BaseNode _startNode;

        public Flow(BaseNode startNode = null)
        {
            _startNode = startNode;
        }

        public BaseNode Start(BaseNode startNode)
        {
            _startNode = startNode;
            return startNode;
        }

        public BaseNode GetNextNode(BaseNode currentNode, string action)
        {
            action = action ?? "default";

            if (currentNode.Successors.TryGetValue(action, out var nextNode))
            {
                return nextNode;
            }

            if (currentNode.Successors.Count > 0)
            {
                var availableActions = string.Join(", ", currentNode.Successors.Keys);
                Console.WriteLine($"Warning: Flow ends: '{action}' not found in [{availableActions}]");
            }

            return null;
        }

        protected object _Orchestrate(Dictionary<string, object> shared, Dictionary<string, object> parameters = null)
        {
            var currentNode = DeepCopy(_startNode);
            var nodeParams = parameters ?? new Dictionary<string, object>(_params);
            object lastAction = null;

            while (currentNode != null)
            {
                currentNode.SetParams(nodeParams);
                lastAction = currentNode.RunInternal(shared);
                currentNode = DeepCopy(GetNextNode(currentNode, lastAction as string));
            }

            return lastAction;
        }

        protected override object _Run(Dictionary<string, object> shared)
        {
            var prepResult = Prep(shared);
            var orchestrationResult = _Orchestrate(shared);
            return Post(shared, prepResult, orchestrationResult);
        }

        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            return execResult;
        }

        // Helper method to simulate deep copy behavior
        private BaseNode DeepCopy(BaseNode node)
        {
            // This is a simplified deep copy - in a real implementation
            // you might want to use serialization or a more sophisticated approach
            return node;
        }
    }
}
