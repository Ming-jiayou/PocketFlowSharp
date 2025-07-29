using System;
using System.Windows.Threading;
using PocketFlowSharpGallery.ViewModels.Pages;
using System.Collections.ObjectModel;

namespace PocketFlowSharpGallery.Services
{
    /// <summary>
    /// WPF专用的进度报告实现，确保线程安全
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

        public void ReportProgress(string step, string message, int progress)
        {
            _dispatcher.Invoke(() =>
            {
                _viewModel.SearchStatus = message;
                _viewModel.ProgressValue = Math.Clamp(progress, 0, 100);
                _viewModel.CurrentStep = step;
            });
        }

        public void ReportIntermediateResult(string type, string content)
        {
            _dispatcher.Invoke(() =>
            {
                var icon = GetIconForType(type);
                var formattedMessage = $"[{DateTime.Now:HH:mm:ss}] {icon} {content}";
                
                _viewModel.IntermediateMessages.Insert(0, formattedMessage);
                
                // 限制消息数量，避免内存泄漏
                if (_viewModel.IntermediateMessages.Count > 100)
                {
                    _viewModel.IntermediateMessages.RemoveAt(_viewModel.IntermediateMessages.Count - 1);
                }
            });
        }

        public void ReportError(string error)
        {
            _dispatcher.Invoke(() =>
            {
                var formattedError = $"[{DateTime.Now:HH:mm:ss}] ❌ Error: {error}";
                _viewModel.IntermediateMessages.Insert(0, formattedError);
                _viewModel.SearchStatus = "Error occurred";
            });
        }

        public void ReportComplete(string result)
        {
            _dispatcher.Invoke(() =>
            {
                _viewModel.SearchStatus = "Completed";
                _viewModel.ProgressValue = 100;
                _viewModel.Result = result;
            });
        }

        private string GetIconForType(string type)
        {
            return type.ToLower() switch
            {
                "decision" => "🤔",
                "search" => "🔍",
                "answer" => "✏️",
                "info" => "ℹ️",
                "warning" => "⚠️",
                _ => "📌"
            };
        }
    }
}