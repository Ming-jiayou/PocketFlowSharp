using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharp
{
    public class AsyncFlow : Flow, IAsyncNode
    {
        public AsyncFlow(BaseNode startNode = null) : base(startNode)
        {
        }

        protected async Task<object> _OrchAsync(Dictionary<string, object> shared, Dictionary<string, object> parameters = null)
        {
            var currentNode = DeepCopy(_startNode);
            var nodeParams = parameters ?? new Dictionary<string, object>(_params);
            object lastAction = null;

            while (currentNode != null)
            {
                currentNode.SetParams(nodeParams);
                lastAction = currentNode is AsyncNode asyncNode
                    ? await asyncNode.RunAsync(shared)
                    : currentNode.RunInternal(shared);
                currentNode = DeepCopy(GetNextNode(currentNode, lastAction as string));
            }

            return lastAction;
        }

        public async Task<object> RunAsync(Dictionary<string, object> shared)
        {
            var prepResult = await PrepAsync(shared);
            var orchestrationResult = await _OrchAsync(shared);
            return await PostAsync(shared, prepResult, orchestrationResult);
        }

        protected virtual Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task<object> PostAsync(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            return Task.FromResult(execResult);
        }

        // Override synchronous methods to prevent their use
        protected override object _Run(Dictionary<string, object> shared)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        // Helper method to simulate deep copy behavior (same as in Flow)
        private BaseNode DeepCopy(BaseNode node)
        {
            return node;
        }
    }
} 