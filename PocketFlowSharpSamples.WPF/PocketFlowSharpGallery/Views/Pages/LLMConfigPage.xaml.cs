using PocketFlowSharpGallery.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;

namespace PocketFlowSharpGallery.Views.Pages
{
    /// <summary>
    /// LLMConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class LLMConfigPage : Page
    {
        private readonly LLMConfigViewModel _viewModel;

        public LLMConfigPage(LLMConfigViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
        }

        private async void LLMConfigPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadConfigsCommand.ExecuteAsync(null);
        }
    }
}
