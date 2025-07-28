using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;
using PocketFlowSharpGallery.Views.Dialogs;
using PocketFlowSharpGallery.Models;
using PocketFlowSharpGallery.ViewModels.Dialogs;

namespace PocketFlowSharpGallery.Services
{
    /// <summary>
    /// 对话框服务实现
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// 显示删除确认对话框
        /// </summary>
        /// <param name="parameters">删除确认参数</param>
        /// <returns>用户确认结果</returns>
        public async Task<bool> ShowDeleteConfirmationAsync(DeleteConfirmationParameters parameters)
        {
            var viewModel = new DeleteConfirmationViewModel();
            viewModel.SetParameters(parameters);

            var dialog = new DeleteConfirmationDialog
            {
                DataContext = viewModel
            };

            // 设置对话框的所有者窗口
            if (Application.Current?.MainWindow != null)
            {
                dialog.Owner = Application.Current.MainWindow;
            }

            // 显示对话框并等待结果
            var result = await dialog.ShowDialogAsync();
            return result ?? false;
        }

        /// <summary>
        /// 显示删除确认对话框（简化版本）
        /// </summary>
        /// <param name="itemType">项目类型</param>
        /// <param name="itemName">项目名称</param>
        /// <returns>用户确认结果</returns>
        public async Task<bool> ShowDeleteConfirmationAsync(string itemType, string itemName)
        {
            return await ShowDeleteConfirmationAsync(new DeleteConfirmationParameters
            {
                ItemType = itemType,
                ItemName = itemName
            });
        }

        /// <summary>
        /// 显示删除确认对话框（兼容旧版本）
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>用户确认结果</returns>
        public async Task<bool> ShowDeleteConfirmationAsync(string configName)
        {
            return await ShowDeleteConfirmationAsync(new DeleteConfirmationParameters
            {
                ItemType = "配置",
                ItemName = configName
            });
        }
    }
}