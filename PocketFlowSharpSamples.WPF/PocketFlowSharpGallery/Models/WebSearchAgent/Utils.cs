using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PocketFlowSharpGallery.Models.WebSearchAgent
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

        public static async Task<string> CallLLMAsync(string prompt)
        {
            try
            {
                ApiKeyCredential apiKeyCredential = new ApiKeyCredential(ApiKey);

                OpenAIClientOptions openAIClientOptions = new OpenAIClientOptions();
                openAIClientOptions.Endpoint = new Uri(EndPoint);

                ChatClient client = new(model: ModelName, apiKeyCredential, openAIClientOptions);

                ChatCompletion completion = await client.CompleteChatAsync(prompt);
                
                return completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                return $"Error calling LLM: {ex.Message}";
            }
        }

        // 使用静态 HttpClient 实例以避免端口耗尽和性能问题
        private static readonly HttpClient _httpClient = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            // 设置超时时间为 30 秒
            client.Timeout = TimeSpan.FromSeconds(30);
            return client;
        }

        public static string SearchWebSync(string query)
        {
            // 使用 Task.Run 在线程池线程上执行异步操作，避免死锁
            return Task.Run(() => SearchWeb(query)).GetAwaiter().GetResult();
        }

        public static async Task<string> SearchWebAsync(string query)
        {
            return await SearchWeb(query);
        }

        public static async Task<string> SearchWeb(string query)
        {
            string apiKey = BraveSearchApiKey;

            // Set the request URL
            string url = $"https://api.search.brave.com/res/v1/web/search?q={query}";

            try
            {
                // 清除之前的请求头
                _httpClient.DefaultRequestHeaders.Clear();
                
                // Set request headers
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("x-subscription-token", apiKey);

                // Send GET request with ConfigureAwait(false) 避免上下文切换问题
                HttpResponseMessage response = await _httpClient.GetAsync(url).ConfigureAwait(false);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Request succeeded, parse the returned JSON data
                    string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    JsonElement root = doc.RootElement;
                    JsonElement results = root.GetProperty("web").GetProperty("results");

                    // Build the results string
                    StringBuilder resultsStr = new StringBuilder();
                    foreach (JsonElement searchResult in results.EnumerateArray())
                    {
                        string title = searchResult.GetProperty("title").GetString() ?? "";
                        string resultUrl = searchResult.GetProperty("url").GetString() ?? "";
                        string description = searchResult.GetProperty("description").GetString() ?? "";

                        resultsStr.AppendLine($"Title: {title}");
                        resultsStr.AppendLine($"URL: {resultUrl}");
                        resultsStr.AppendLine($"Description: {description}");
                        resultsStr.AppendLine();
                    }
                    
                    string finalResult = resultsStr.ToString();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(finalResult);
                    Console.ForegroundColor = ConsoleColor.Green;
                    return finalResult;
                }
                else
                {
                    // Request failed, print error message
                    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                    return $"Error: Search request failed with status code {response.StatusCode}";
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timed out: {ex.Message}");
                return "Error: Search request timed out after 30 seconds";
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                return $"Error: HTTP request failed - {ex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return $"Error: Unexpected error occurred - {ex.Message}";
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