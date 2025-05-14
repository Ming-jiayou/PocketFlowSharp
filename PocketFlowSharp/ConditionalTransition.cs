using System;
using System.Collections.Generic;
using System.Text;

namespace PocketFlowSharp
{
    public class ConditionalTransition
    {
        private readonly BaseNode _source;
        private readonly string _action;

        public ConditionalTransition(BaseNode source, string action)
        {
            _source = source;
            _action = action;
        }

        public BaseNode Source => _source;
        public string Action => _action;

        public static BaseNode operator -(ConditionalTransition transition, BaseNode target)
        {
            return transition._source.Next(target, transition._action);
        }
    }
}
