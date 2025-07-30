using System;

namespace PocketFlowSharpGallery.Services
{
    /// <summary>
    /// 简化的进度报告接口，仅用于将节点信息打印到界面
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// 打印信息到结果区域
        /// </summary>
        /// <param name="message">要显示的消息</param>
        void PrintMessage(string message);
             
        /// <summary>
        /// 清空结果区域
        /// </summary>
        void Clear();
    }
}