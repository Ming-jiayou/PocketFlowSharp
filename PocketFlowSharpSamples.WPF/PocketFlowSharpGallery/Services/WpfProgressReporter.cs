using System;
using System.Windows.Threading;
using PocketFlowSharpGallery.ViewModels.Pages;

namespace PocketFlowSharpGallery.Services
{
    /// <summary>
    /// 简化的WPF进度报告实现，直接将信息打印到结果控件
    /// </summary>
    public class WpfProgressReporter : IProgressReporter
    {
        private readonly Dispatcher _dispatcher;
        private readonly WebSearchViewModel _viewModel;

        public WpfProgressReporter(WebSearchViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void PrintMessage(string message)
        {
            _dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(_viewModel.Result))
                {
                    _viewModel.Result += Environment.NewLine;
                }
                _viewModel.Result += message;
            });
        }
    
        public void Clear()
        {
            _dispatcher.Invoke(() =>
            {
                _viewModel.Result = string.Empty;
            });
        }
    }
}