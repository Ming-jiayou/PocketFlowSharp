using System.Threading.Tasks;

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
        /// <param name="configName">配置名称</param>
        /// <returns>用户确认结果</returns>
        Task<bool> ShowDeleteConfirmationAsync(string configName);
    }
}