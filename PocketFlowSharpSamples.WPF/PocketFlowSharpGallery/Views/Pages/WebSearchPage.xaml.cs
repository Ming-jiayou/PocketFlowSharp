using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PocketFlowSharpGallery.ViewModels.Pages;

namespace PocketFlowSharpGallery.Views.Pages
{
    /// <summary>
    /// Interaction logic for WebSearchPage.xaml
    /// </summary>
    public partial class WebSearchPage : Page
    {
        public WebSearchViewModel ViewModel { get; }

        public WebSearchPage(WebSearchViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel.SearchCommand.CanExecute(null))
                {
                    ViewModel.SearchCommand.Execute(null);
                }
            }
        }
    }
}
