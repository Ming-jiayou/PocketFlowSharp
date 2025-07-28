using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PocketFlowSharpGallery.WebSearchAgent
{
    public static class Utils
    {
        public static string ModelName { get; set; } = "";
        public static string EndPoint { get; set; } = "";
        public static string ApiKey { get; set; } = "";
        public static string BraveSearchApiKey { get; set; } = "";

        public static string CallLLM(string prompt)
        {
            try
            {
                ApiKeyCredential apiKeyCredential = new ApiKeyCredential(ApiKey);

                OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions();
                openAIClientOptions.Endpoint = new Uri(EndPoint);

                ChatClient client = new(model: ModelName, apiKeyCredential, openAIClientOptions);

                ChatCompletion completion = client.CompleteChat(prompt);

                return completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                return $"Error calling LLM: {ex.Message}";
            }
        }

        public static string SearchWebSync(string query)
        {
            return SearchWeb(query).GetAwaiter().GetResult();
        }

        public static async Task<string> SearchWeb(string query)
        {
            string apiKey = BraveSearchApiKey;

            if (string.IsNullOrEmpty(apiKey))
            {
                return "Error: Brave Search API key not configured";
            }

            // Set the request URL
            string url = $"https://api.search.brave.com/res/v1/web/search?q={Uri.EscapeDataString(query)}";

            // Create an HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set request headers
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("x-subscription-token", apiKey);

                    // Send GET request
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Request succeeded, parse the returned JSON data
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                        JsonElement root = doc.RootElement;
                        JsonElement results = root.GetProperty("web").GetProperty("results");

                        // Build the results string
                        string resultsStr = "";
                        foreach (JsonElement result in results.EnumerateArray())
                        {
                            string title = result.GetProperty("title").GetString() ?? "";
                            string resultUrl = result.GetProperty("url").GetString() ?? "";
                            string description = result.GetProperty("description").GetString() ?? "";

                            resultsStr += $"Title: {title}\nURL: {resultUrl}\nDescription: {description}\n\n";
                        }
                        return resultsStr;
                    }
                    else
                    {
                        // Request failed, print error message
                        return $"Error: Search request failed with status code {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error: Search request failed with exception {ex.Message}";
                }
            }
        }

        // Simple YAML parser implementation to avoid external dependencies
        public static Dictionary<string, object> ParseSimpleYaml(string yaml)
        {
            var result = new Dictionary<string, object>();
            string[] lines = yaml.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;

                // Parse key-value pairs
                int colonIndex = line.IndexOf(':');
                if (colonIndex > 0)
                {
                    string key = line.Substring(0, colonIndex).Trim();
                    string value = line.Substring(colonIndex + 1).Trim();

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

                        result[key] = multiline.ToString().Trim();
                    }
                    else
                    {
                        // Simple key-value
                        result[key] = value;
                    }
                }
            }

            return result;
        }
    }
} 