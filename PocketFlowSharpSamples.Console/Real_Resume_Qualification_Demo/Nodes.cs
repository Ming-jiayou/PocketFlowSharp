using PocketFlowSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig.Core;

namespace Real_Resume_Qualification_Demo
{
    /// <summary>
    /// Map阶段：从数据目录读取所有简历到共享存储中
    /// </summary>
    public class ReadResumesNode : Node
    {
        public override object Exec(object input)
        {
            var resumeFiles = new Dictionary<string, string>();

            string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");

            foreach (string file in Directory.GetFiles(dataDir, "*.pdf"))
            {
                string filename = Path.GetFileName(file);
                string content = Utils.ExtractTextFromPdf(file);
                resumeFiles[filename] = content;
            }

            return resumeFiles;
        }

        public override object Post(Dictionary<string, object> shared, object prepRes, object execRes)
        {
            shared["resumes"] = execRes;

            return "default";
        }
    }

    /// <summary>
    /// 批处理：评估每份简历以确定候选人是否合格
    /// </summary>
    public class EvaluateResumesNode : BatchNode
    {
        string? requirements = "";
        public override object Prep(Dictionary<string, object> shared)
        {
            var resumes = shared["resumes"] as Dictionary<string, string>;
            requirements = shared["requirements"] as string;

            return resumes?.Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value)).ToList();
        }

        public override object Exec(object prepResult)
        {
            if (prepResult is KeyValuePair<string, string> resumeItem)
            {
                string filename = resumeItem.Key;
                string content = resumeItem.Value;
              
                string prompt = $@"
评估以下简历并确定候选人是否符合职位的要求。
资格标准：
{requirements}

简历内容：
{content}

请以YAML格式返回您的评估：
```yaml
candidate_name: [候选人姓名]
qualifies: [true/false]
reasons:
  - [资格认定/不认定的第一个原因]
  - [第二个原因（如果适用）]
```
";
                string response = Utils.CallLLM(prompt);

                // 提取YAML内容
                string yamlContent = response.Contains("```yaml")
                    ? response.Split("```yaml")[1].Split("```")[0].Trim()
                    : response;

                var result = Utils.ParseSimpleYaml(yamlContent);

                return result;
            }

            return base.Exec(prepResult);
        }

        public override object Post(Dictionary<string, object> shared, object prepRes, object execRes)
        {
            shared["evaluations"] = execRes;
            return "default";
        }
    }

    /// <summary>
    /// Reduce节点：统计并打印出有多少候选人符合要求
    /// </summary>
    public class ReduceResultsNode : Node
    {
        public override object Prep(Dictionary<string, object> shared)
        {
            return shared["evaluations"];
        }

        public override object Exec(object prepResult)
        {
            List<Dictionary<string, object>> evaluations = null;
            var objectList = prepResult as List<object>;

            if (objectList != null)
            {
                evaluations = objectList
                    .Select(item => item as Dictionary<string, object>)
                    .Where(dict => dict != null)
                    .ToList();
            }

            int qualifiedCount = 0;
            int notPassedCount = 0;
            int totalCount = evaluations?.Count ?? 0;
            var qualifiedCandidates = new List<string>();
            var notPassedCandidates = new List<string>();

            if (evaluations != null)
            {
                foreach (var evaluation in evaluations)
                {
                    if (evaluation.TryGetValue("qualifies", out var qualifiesValue) && qualifiesValue.ToString() == "true")
                    {
                        qualifiedCount++;

                        string name = evaluation.TryGetValue("candidate_name", out var candidateNameValue)
                            ? candidateNameValue?.ToString() ?? "未知"
                            : "未知";

                        qualifiedCandidates.Add(name);
                    }
                    if (evaluation.TryGetValue("qualifies", out var notPassedValue) && notPassedValue.ToString() == "false")
                    {
                        notPassedCount++;

                        string name = evaluation.TryGetValue("candidate_name", out var candidateNameValue)
                            ? candidateNameValue?.ToString() ?? "未知"
                            : "未知";

                        notPassedCandidates.Add(name);
                    }
                }
            }

            var summary = new Dictionary<string, object>
            {
                ["total_candidates"] = totalCount,
                ["qualified_count"] = qualifiedCount,
                ["qualified_percentage"] = totalCount > 0 ? Math.Round((double)qualifiedCount / totalCount * 100, 1) : 0,
                ["qualified_names"] = qualifiedCandidates,
                ["not_passed_count"] = notPassedCount,
                ["not_passed_names"] = notPassedCandidates
            };

            return summary;
        }

        public override object Post(Dictionary<string, object> shared, object prepRes, object execRes)
        {
            var summary = execRes as Dictionary<string, object>;
            shared["summary"] = summary;

            Console.WriteLine("\n===== 简历资格评估汇总 =====");
            Console.WriteLine($"评估的总候选人数：{summary["total_candidates"]}");
            Console.WriteLine($"合格的候选人数：{summary["qualified_count"]} ({summary["qualified_percentage"]}%)");

            var qualifiedNames = summary["qualified_names"] as List<string>;
            if (qualifiedNames?.Any() == true)
            {
                Console.WriteLine("\n合格的候选人：");
                foreach (var name in qualifiedNames)
                {
                    Console.WriteLine($"- {name}");
                }
            }

            var notPassedNames = summary["not_passed_names"] as List<string>;
            if (notPassedNames?.Any() == true)
            {
                Console.WriteLine("\n没通过的候选人：");
                foreach (var name in notPassedNames)
                {
                    Console.WriteLine($"- {name}");
                }
            }

            List<Dictionary<string, object>> evaluations = null;
            var objectList = shared["evaluations"] as List<object>;

            if (objectList != null)
            {
                evaluations = objectList
                    .Select(item => item as Dictionary<string, object>)
                    .Where(dict => dict != null)
                    .ToList();
            }

            if (evaluations != null && evaluations.Any())
            {
                Console.WriteLine("\n详细评估结果：");
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var evaluation in evaluations)
                {
                    string name = evaluation.TryGetValue("candidate_name", out var candidateNameValue)
                        ? candidateNameValue?.ToString() ?? "未知"
                        : "未知";
                    var qualifies = evaluation.TryGetValue("qualifies", out var qualifiesValue);
                    string status = qualifiesValue.ToString() == "true" ? "合格" : "不合格";
                    Console.WriteLine($"{status} - {name}");
                    string resons = evaluation.TryGetValue("reasons", out var reasonsValue)
                        ? string.Join("",(List<string>)reasonsValue)
                        : "无具体原因";
                    Console.WriteLine($"原因: {resons}");
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.White;
            }

            return "default";
        }
    }
}
