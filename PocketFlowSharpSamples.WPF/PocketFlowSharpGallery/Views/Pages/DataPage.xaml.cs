using PocketFlowSharpGallery.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace PocketFlowSharpGallery.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
