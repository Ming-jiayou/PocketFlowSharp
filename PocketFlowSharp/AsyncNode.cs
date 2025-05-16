using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PocketFlowSharp
{
    public abstract class AsyncNode : Node
    {
        protected int MaxRetries { get; }
        protected int WaitMilliseconds { get; }

        protected AsyncNode(int maxRetries = 3, int waitMilliseconds = 0) : base(1, 0)
        {
            MaxRetries = maxRetries;
            WaitMilliseconds = waitMilliseconds;
        }

        protected virtual Task<object> PrepAsync(Dictionary<string, object> shared)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task<object> ExecAsync(object prepRes)
        {
            return Task.FromResult<object>(null);
        }

        protected virtual Task<object> ExecFallbackAsync(object prepRes, Exception exc)
        {
            return Task.FromException<object>(exc);
        }

        protected virtual Task<object> PostAsync(Dictionary<string, object> shared, object prepRes, object execRes)
        {
            return Task.FromResult<object>(null);
        }

        protected async Task<object> _ExecAsync(object prepRes)
        {
            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    return await ExecAsync(prepRes);
                }
                catch (Exception e)
                {
                    if (i == MaxRetries - 1)
                    {
                        return await ExecFallbackAsync(prepRes, e);
                    }

                    if (WaitMilliseconds > 0)
                    {
                        await Task.Delay(WaitMilliseconds);
                    }
                }
            }
            throw new InvalidOperationException("Execution failed with no fallback result");
        }

        protected async Task<object> _RunAsync(Dictionary<string, object> shared)
        {
            var prepResult = await PrepAsync(shared);
            var execResult = await _ExecAsync(prepResult);
            return await PostAsync(shared, prepResult, execResult);
        }

        public async Task<object> RunAsync(Dictionary<string, object> shared)
        {          
            return await _RunAsync(shared);
        }

        #region Override Sync Methods to Throw Exceptions

        public override object Prep(Dictionary<string, object> shared)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        public override object Exec(object prepResult)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        public override object ExecFallback(object prepResult, Exception exception)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        protected override object _Run(Dictionary<string, object> shared)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        protected override object _Exec(object prepResult)
        {
            throw new InvalidOperationException("Use RunAsync instead of synchronous methods.");
        }

        #endregion
    }
} 