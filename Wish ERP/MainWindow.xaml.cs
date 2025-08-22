using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wish_ERP.Models;
using Wish_ERP.Services;

namespace Wish_ERP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.ClientsViewModel();
            ChangeCurrentPage(Views.OrderView.Instance);
            _ = InitializeAsync();
        }
        private async Task InitializeAsync()
        {
            Load();
        }
        private void Load()
        {

            textblock.TextWrapping = TextWrapping.Wrap;
            textblock.Text = DataManager.Instance.Orders.FirstOrDefault().Id.ToString();
        }

        private void OpenClientView(object sender, RoutedEventArgs e)
        {
            ChangeCurrentPage(CurrentPage.Content = Views.ClientsView.Instance);
        }
        private void OpenProductsView(object sender, RoutedEventArgs e)
        {
            ChangeCurrentPage(Views.ProductsView.Instance);
        }
        private void OpenOrdersView(object sender, RoutedEventArgs e)
        {
            ChangeCurrentPage(Views.OrderView.Instance);
        }


        private void ChangeCurrentPage(object Page)
        {
            if (CurrentPage.Content == Page)return;
            
            CurrentPage.Content = Page;
        }

    }
}