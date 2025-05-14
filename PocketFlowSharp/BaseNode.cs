using System;
using System.Collections.Generic;
using System.Text;

namespace PocketFlowSharp
{
    public class BaseNode
    {
        protected Dictionary<string, object> _params = new Dictionary<string, object>();
        protected Dictionary<string, BaseNode> _successors = new Dictionary<string, BaseNode>();

        public Dictionary<string, object> Params
        {
            get { return _params; }
            set { _params = value; }
        }

        public Dictionary<string, BaseNode> Successors
        {
            get { return _successors; }
        }

        public void SetParams(Dictionary<string, object> parameters)
        {
            _params = parameters;
        }

        public BaseNode Next(BaseNode node, string action = "default")
        {
            if (_successors.ContainsKey(action))
            {
                Console.WriteLine($"Warning: Overwriting successor for action '{action}'");
            }

            _successors[action] = node;
            return node;
        }

        public virtual object Prep(Dictionary<string, object> shared)
        {
            return null;
        }

        public virtual object Exec(object prepResult)
        {
            return null;
        }

        public virtual object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            return null;
        }

        protected virtual object _Exec(object prepResult)
        {
            return Exec(prepResult);
        }

        protected virtual object _Run(Dictionary<string, object> shared)
        {
            var prepResult = Prep(shared);
            var execResult = _Exec(prepResult);
            return Post(shared, prepResult, execResult);
        }

        public object Run(Dictionary<string, object> shared)
        {
            if (_successors.Count > 0)
            {
                Console.WriteLine("Warning: Node won't run successors. Use Flow.");
            }

            return _Run(shared);
        }

        // Method to allow access to _Run from Flow class
        public object RunInternal(Dictionary<string, object> shared)
        {
            return _Run(shared);
        }

        public static BaseNode operator -(BaseNode node, string action)
        {
            node.Next(node, action);
            return node;
        }

        public static BaseNode operator +(BaseNode left, BaseNode right)
        {
            return left.Next(right);
        }       
    }
}
