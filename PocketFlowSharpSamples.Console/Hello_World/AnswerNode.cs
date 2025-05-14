using PocketFlowSharp;
using System;
using System.Collections.Generic;

namespace Hello_World
{
    public class AnswerNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // Read question from shared
            return shared["question"];
        }
        
        public override object Exec(object prepResult)
        {
            // Call LLM with the question
            string question = (string)prepResult;
            return Utils.CallLLM(question);
        }
        
        public override object Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // Store the answer in shared
            shared["answer"] = execResult;
            return "done";
        }
    }
} 