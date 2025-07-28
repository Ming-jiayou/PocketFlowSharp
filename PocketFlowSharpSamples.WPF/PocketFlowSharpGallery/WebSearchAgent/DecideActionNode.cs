using PocketFlowSharp;

namespace PocketFlowSharpGallery.WebSearchAgent
{
    public class DecideActionNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            // Prepare the context and question for the decision-making process
            string context = shared.ContainsKey("context") ? (string)shared["context"] : "No previous search";
            string question = (string)shared["question"];

            // Return both for the exec step (as a tuple)
            return new Tuple<string, string>(question, context);
        }

        public override object Exec(object inputs)
        {
            // Call the LLM to decide whether to search or answer
            var (question, context) = (Tuple<string, string>)inputs;

            // Create a prompt to help the LLM decide what to do next
            string prompt = $@"
### CONTEXT
You are a research assistant that can search the web.
Question: {question}
Previous Research: {context}

### ACTION SPACE
[1] search
  Description: Look up more information on the web
  Parameters:
    - query (str): What to search for

[2] answer
  Description: Answer the question with current knowledge
  Parameters:
    - answer (str): Final answer to the question

## NEXT ACTION
Decide the next action based on the context and available actions.
Return your response in this format:

```yaml
thinking: |
    <your step-by-step reasoning process>
action: search OR answer
reason: <why you chose this action>
answer: <if action is answer>
search_query: <specific search query if action is search>
```
IMPORTANT: Make sure to:
1. Use proper indentation (4 spaces) for all multi-line fields
2. Use the | character for multi-line text fields
3. Keep single-line fields without the | character
";

            // Call the LLM to make a decision
            string response = Utils.CallLLM(prompt);

            // Parse the response to get the decision
            try
            {
                string yamlStr = response.Split("```yaml")[1].Split("```")[0].Trim();
                var decision = Utils.ParseSimpleYaml(yamlStr);
                return decision;
            }
            catch
            {
                // Fallback decision if parsing fails
                return new Dictionary<string, object>
                {
                    { "action", "answer" },
                    { "reason", "Failed to parse LLM response" },
                    { "answer", "Sorry, I encountered an error processing your request." }
                };
            }
        }

        public override string Post(Dictionary<string, object> shared, object prepResult, object execResult)
        {
            // Save the decision and determine the next step in the flow
            var decision = (Dictionary<string, object>)execResult;

            // If LLM decided to search, save the search query
            if (decision["action"].ToString() == "search")
            {
                shared["search_query"] = decision["search_query"];
            }
            else
            {
                shared["context"] = decision["answer"]; // Save the context if LLM gives the answer without searching
            }

            // Return the action to determine the next node in the flow
            return decision["action"]?.ToString() ?? "answer";
        }
    }
} 