using OpenAI.Chat;
using OpenAI;
using System;
using System.ClientModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Resume_Qualification_Demo
{
    public static class Utils
    {
        public static string ModelName { get; set; }
        public static string EndPoint { get; set; }
        public static string ApiKey { get; set; }
        public static string BraveSearchApiKey { get; set; }

        public static string CallLLM(string prompt)
        {
            ApiKeyCredential apiKeyCredential = new ApiKeyCredential(ApiKey);

            OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions();
            openAIClientOptions.Endpoint = new Uri(EndPoint);

            ChatClient client = new(model: ModelName, apiKeyCredential, openAIClientOptions);

            ChatCompletion completion = client.CompleteChat(prompt);

            return completion.Content[0].Text;
        }

        // Simple YAML parser implementation to avoid external dependencies
        public static Dictionary<string, object> ParseSimpleYaml(string yaml)
        {
            var result = new Dictionary<string, object>();
            string[] lines = yaml.Split('\n');
            string currentKey = null;
            List<string> currentList = null;
            int currentIndentation = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;

                int indentation = lines[i].TakeWhile(c => c == ' ').Count();

                // Handle list items
                if (line.StartsWith("- "))
                {
                    if (currentList == null)
                    {
                        currentList = new List<string>();
                        result[currentKey] = currentList;
                    }
                    string listItem = line.Substring(2).Trim();
                    currentList.Add(listItem);
                    continue;
                }

                // Parse key-value pairs
                int colonIndex = line.IndexOf(':');
                if (colonIndex > 0)
                {
                    currentKey = line.Substring(0, colonIndex).Trim();
                    string value = line.Substring(colonIndex + 1).Trim();
                    currentIndentation = indentation;
                    currentList = null;

                    // Check if this is a multi-line value with |
                    if (value == "|")
                    {
                        StringBuilder multiline = new StringBuilder();
                        i++;

                        // Collect all indented lines
                        while (i < lines.Length && (lines[i].StartsWith("    ") || string.IsNullOrWhiteSpace(lines[i])))
                        {
                            if (!string.IsNullOrWhiteSpace(lines[i]))
                                multiline.AppendLine(lines[i].Substring(4)); // Remove indentation
                            i++;
                        }
                        i--; // Step back to process the next non-indented line in the outer loop

                        result[currentKey] = multiline.ToString().Trim();
                    }
                    else if (!string.IsNullOrEmpty(value))
                    {
                        // Simple key-value
                        result[currentKey] = value;
                    }
                }
            }

            return result;
        }
    }
}