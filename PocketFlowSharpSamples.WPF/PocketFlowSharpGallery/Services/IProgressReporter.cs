using System;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Services
{
    /// <summary>
    /// 进度报告接口，用于在异步操作中报告进度和中间结果
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// 报告进度更新
        /// </summary>
        /// <param name="step">当前步骤</param>
        /// <param name="message">进度消息</param>
        /// <param name="progress">进度百分比 (0-100)</param>
        void ReportProgress(string step, string message, int progress);

        /// <summary>
        /// 报告中间结果
        /// </summary>
        /// <param name="type">结果类型 (decision, search, answer)</param>
        /// <param name="content">结果内容</param>
        void ReportIntermediateResult(string type, string content);

        /// <summary>
        /// 报告错误信息
        /// </summary>
        /// <param name="error">错误消息</param>
        void ReportError(string error);

        /// <summary>
        /// 报告操作完成
        /// </summary>
        /// <param name="result">最终结果</param>
        void ReportComplete(string result);
    }

    /// <summary>
    /// 进度更新数据模型
    /// </summary>
    public class ProgressUpdate
    {
        public string Step { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Progress { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 中间结果数据模型
    /// </summary>
    public class IntermediateResult
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}