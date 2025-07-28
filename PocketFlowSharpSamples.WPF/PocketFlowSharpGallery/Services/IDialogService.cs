using System.Threading.Tasks;
using PocketFlowSharpGallery.Models;

namespace PocketFlowSharpGallery.Services
{
    /// <summary>
    /// 对话框服务接口
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// 显示删除确认对话框
        /// </summary>
        /// <param name="parameters">删除确认参数</param>
        /// <returns>用户确认结果</returns>
        Task<bool> ShowDeleteConfirmationAsync(DeleteConfirmationParameters parameters);

        /// <summary>
        /// 显示删除确认对话框（简化版本）
        /// </summary>
        /// <param name="itemType">项目类型</param>
        /// <param name="itemName">项目名称</param>
        /// <returns>用户确认结果</returns>
        Task<bool> ShowDeleteConfirmationAsync(string itemType, string itemName);
    }
}