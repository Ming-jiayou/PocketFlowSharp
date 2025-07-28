using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wpf.Ui.Controls;
using PocketFlowSharpGallery.Models;

namespace PocketFlowSharpGallery.ViewModels.Dialogs
{
    /// <summary>
    /// 删除确认对话框视图模型
    /// </summary>
    public partial class DeleteConfirmationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "确认删除";

        [ObservableProperty]
        private string _itemType = "项目";

        [ObservableProperty]
        private string _itemName = string.Empty;

        [ObservableProperty]
        private string _warningMessage = "此操作不可撤销，删除后将无法恢复。";

        [ObservableProperty]
        private string _confirmButtonText = "删除";

        [ObservableProperty]
        private string _cancelButtonText = "取消";

        [ObservableProperty]
        private bool _isConfirmed;

        /// <summary>
        /// 设置删除确认参数
        /// </summary>
        public void SetParameters(DeleteConfirmationParameters parameters)
        {
            Title = parameters.Title;
            ItemType = parameters.ItemType;
            ItemName = parameters.ItemName;
            WarningMessage = parameters.WarningMessage;
            ConfirmButtonText = parameters.ConfirmButtonText;
            CancelButtonText = parameters.CancelButtonText;
        }

        /// <summary>
        /// 确认命令
        /// </summary>
        [RelayCommand]
        private void Confirm()
        {
            IsConfirmed = true;
            CloseDialog(true);
        }

        /// <summary>
        /// 取消命令
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            IsConfirmed = false;
            CloseDialog(false);
        }

        private void CloseDialog(bool result)
        {
            // 通过消息机制通知对话框关闭
            WeakReferenceMessenger.Default.Send(new DialogCloseMessage(result));
        }
    }

    /// <summary>
    /// 对话框关闭消息
    /// </summary>
    public class DialogCloseMessage
    {
        public bool Result { get; }

        public DialogCloseMessage(bool result)
        {
            Result = result;
        }
    }
}