using PocketFlowSharpGallery.ViewModels.Pages;
using System.Windows.Controls;

namespace PocketFlowSharpGallery.Views.Pages
{
    /// <summary>
    /// SearchEngineConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class SearchEngineConfigPage : Page
    {
        public SearchEngineConfigViewModel ViewModel { get; }

        public SearchEngineConfigPage(SearchEngineConfigViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            InitializeComponent();
        }

        private async void SearchEngineConfigPage_OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }
    }
} 