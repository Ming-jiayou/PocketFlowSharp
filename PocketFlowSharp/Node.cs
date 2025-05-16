using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PocketFlowSharp
{
    public class Node : BaseNode
    {
        private readonly int _maxRetries;
        private readonly int _waitMilliseconds;
        public int CurrentRetry { get; private set; }

        public Node(int maxRetries = 1, int waitMilliseconds = 0)
        {
            _maxRetries = maxRetries;
            _waitMilliseconds = waitMilliseconds;
        }

        public virtual object ExecFallback(object prepResult, Exception exception)
        {
            throw exception;
        }

        protected override object _Exec(object prepResult)
        {
            for (CurrentRetry = 0; CurrentRetry < _maxRetries; CurrentRetry++)
            {
                try
                {
                    return Exec(prepResult);
                }
                catch (Exception e)
                {
                    if (CurrentRetry == _maxRetries - 1)
                    {
                        return ExecFallback(prepResult, e);
                    }

                    if (_waitMilliseconds > 0)
                    {
                        Thread.Sleep(_waitMilliseconds);
                    }
                }
            }

            // This should never happen because of the fallback
            throw new InvalidOperationException("Execution failed with no fallback result");
        }       
    }
}
