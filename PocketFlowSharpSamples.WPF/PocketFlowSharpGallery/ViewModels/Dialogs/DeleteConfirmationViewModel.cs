using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wpf.Ui.Controls;

namespace PocketFlowSharpGallery.ViewModels.Dialogs
{
    /// <summary>
    /// 删除确认对话框视图模型
    /// </summary>
    public partial class DeleteConfirmationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _configName = string.Empty;

        [ObservableProperty]
        private bool _isConfirmed;

        /// <summary>
        /// 确认命令
        /// </summary>
        public ICommand ConfirmCommand { get; }

        /// <summary>
        /// 取消命令
        /// </summary>
        public ICommand CancelCommand { get; }

        public DeleteConfirmationViewModel()
        {
            ConfirmCommand = new RelayCommand(OnConfirm);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void OnConfirm()
        {
            IsConfirmed = true;
            CloseDialog(true);
        }

        private void OnCancel()
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