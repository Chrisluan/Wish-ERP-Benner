using System;
using System.Collections.Generic;
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
using Wish.ERP.Benner.Views.Modals;
using Wish.ERP.Benner.Views.Modals.Orders;

namespace Wish_ERP.Views
{
    public partial class OrderView : UserControl
    {
        private static OrderView instance = null;
        private List<Order> selectedOrders;


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
        private void AddClient(object sender, RoutedEventArgs e)
        {
            Client newClient = new Client("", "", "");

        }
        private void DeleteClient(object sender, RoutedEventArgs e)
        {


            var selectedOrders = OrdersList.SelectedItems.Cast<Order>().ToList();
            if (selectedOrders.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja excluir {selectedOrders[0].Id}? Essa ação é Irreversível", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;
                OrdersServices.Delete(selectedOrders[0].Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja excluir esses {selectedOrders.Count} clientes? Essa ação é Irreversível", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No) return;
                var ids = selectedOrders.Select(c => c.Id).ToList();
               OrdersServices.DeleteMany(ids);
            }
        }

        private void HandleSelectionList(object sender, SelectionChangedEventArgs e)
        {
            selectedOrders = OrdersList.SelectedItems.Cast<Order>().ToList();
            DeleteClientButton.IsEnabled = selectedOrders.Any();
            EditClientButton.IsEnabled = selectedOrders.Count == 1;
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            var createOrderModal = new CreateOrderModal();
            createOrderModal.ShowDialog();

        }

        private void EditClient(object sender, RoutedEventArgs e)
        {
            if (OrdersList.SelectedItem is Client selectedClient)
            {
                //var editClientModal = new EditClientModal(selectedClient);
                //editClientModal.ShowDialog();
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

