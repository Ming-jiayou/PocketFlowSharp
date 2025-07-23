using PocketFlowSharpGallery.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace PocketFlowSharpGallery.Views.Pages
{
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
