using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using PocketFlowSharpGallery.ViewModels.Dialogs;
using Wpf.Ui.Controls;

namespace PocketFlowSharpGallery.Views.Dialogs
{
    /// <summary>
    /// 删除确认对话框
    /// </summary>
    public partial class DeleteConfirmationDialog : FluentWindow
    {
        private readonly TaskCompletionSource<bool?> _tcs = new();

        public DeleteConfirmationDialog()
        {
            InitializeComponent();
            
            // 注册消息处理
            WeakReferenceMessenger.Default.Register<DialogCloseMessage>(this, OnDialogClose);
            
            // 处理窗口关闭事件
            Closed += OnWindowClosed;
        }

        /// <summary>
        /// 异步显示对话框
        /// </summary>
        /// <returns>对话框结果</returns>
        public async Task<bool?> ShowDialogAsync()
        {
            ShowDialog();
            return await _tcs.Task;
        }

        private void OnDialogClose(object recipient, DialogCloseMessage message)
        {
            _tcs.TrySetResult(message.Result);
            Dispatcher.Invoke(() =>
            {
                DialogResult = message.Result;
                Close();
            });
        }

        private void OnWindowClosed(object? sender, EventArgs e)
        {
            // 清理消息注册
            WeakReferenceMessenger.Default.Unregister<DialogCloseMessage>(this);
            
            // 如果任务还未完成，设置为false
            _tcs.TrySetResult(false);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            
            // 设置窗口样式
            if (Owner != null)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }
    }
}