using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;
using PocketFlowSharpGallery.Views.Dialogs;

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
        /// <param name="configName">配置名称</param>
        /// <returns>用户确认结果</returns>
        public async Task<bool> ShowDeleteConfirmationAsync(string configName)
        {
            var dialog = new DeleteConfirmationDialog
            {
                DataContext = new ViewModels.Dialogs.DeleteConfirmationViewModel
                {
                    ConfigName = configName
                }
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
    }
}