using PocketFlowSharp;

namespace PocketFlowSharpGallery.WebSearchAgent
{
    public class AnswerQuestionNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // Get the question and context for answering
            string question = (string)shared["question"];
            string context = shared.ContainsKey("context") ? (string)shared["context"] : "";
            return new Tuple<string, string>(question, context);
        }

        public override object Exec(object inputs)
        {
            // Call the LLM to generate a final answer
            var (question, context) = (Tuple<string, string>)inputs;

            // Create a prompt for the LLM to answer the question
            string prompt = $@"
### CONTEXT
Based on the following information, answer the question.
Question: {question}
Research: {context}

## YOUR ANSWER:
Provide a comprehensive answer using the research results.
";
            // Call the LLM to generate an answer
            string answer = Utils.CallLLM(prompt);
            return answer;
        }

        public override string Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // Save the final answer and complete the flow
            shared["answer"] = execResult;

            // We're done - no need to continue the flow
            return "done";
        }
    }
} 