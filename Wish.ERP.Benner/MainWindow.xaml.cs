using System;
using System.Collections.Generic;
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
using Wish.ERP.Benner;
namespace Wish_ERP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<Type, string> pageNames = new Dictionary<Type, string>
        {
            { typeof(Views.OrderView), "Serviços" },
            { typeof(Views.ClientsView), "Clientes" },
            { typeof(Views.ProductsView), "Produtos" }
        };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.ClientsViewModel();
            ChangeCurrentPage(Views.OrderView.Instance);
        }


        private void OpenClientView(object sender, RoutedEventArgs e)
        {
            ChangeCurrentPage(Views.ClientsView.Instance);
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
            if (CurrentPage.Content == Page) return;
            CurrentPage.Content = Page;

            ActionBar.Text = pageNames[Page.GetType()];
        }

    }
}