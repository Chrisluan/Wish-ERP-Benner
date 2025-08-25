using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;
using Wish.ERP.Benner.ViewModels;
using Wish.ERP.Benner.Views.Modals;
using Wish.ERP.Benner.Views.Modals.Orders;

namespace Wish_ERP.Views
{
    public partial class OrderView : UserControl
    {
        private static OrderView instance = null;

        public static OrderView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderView();
                }
                return instance;
            }
        }
        public OrderView()
        {
            DataContext = new Wish.ERP.Benner.ViewModels.OrdersViewModel();
            
            InitializeComponent();
        }
        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            if (DataContext is OrdersViewModel ovm)
            {
                var selectedOrders = ovm.SelectedOrders;

                if (!selectedOrders.Any()) return;

                if (selectedOrders.Count == 1)
                {
                    var order = selectedOrders.First();
                    MessageBoxResult result = MessageBox.Show(
                        $"Tem certeza que deseja excluir {order.Id}? Essa ação é irreversível",
                        "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No) return;
                    OrdersServices.Delete(order.Id);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(
                        $"Tem certeza que deseja excluir esses {selectedOrders.Count} clientes? Essa ação é irreversível",
                        "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.No) return;

                    var ids = selectedOrders.Select(c => c.Id).ToList();
                    OrdersServices.DeleteMany(ids);
                }
            }
        }


        private void HandleSelectionList(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is OrdersViewModel ovm)
            {
                ovm.SelectedOrders = OrdersList.SelectedItems.Cast<Order>().ToList();
                DeleteOrderButton.IsEnabled = ovm.SelectedOrders.Any();
            }
        }


        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            var createOrderModal = new CreateOrderModal();
            createOrderModal.ShowDialog();

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]");
            e.Handled = regex.IsMatch(e.Text);
        }



    }
}

