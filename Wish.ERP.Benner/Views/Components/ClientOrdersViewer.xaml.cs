using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;
using Wish.ERP.Benner.ViewModels;
using Wish.ERP.Benner.Views.Modals.Orders;

namespace Wish.ERP.Benner.Views.Components
{
    public partial class ClientOrdersViewer : UserControl
    {

        public ObservableCollection<Order> CurrentSelectedClientOrders { get; set; }

        Client Client { get; set; }
        public ClientOrdersViewer(Client selectedClient)
        {
            InitializeComponent();
            Client = selectedClient;
            
            this.DataContext = new OrdersViewModel();
        }

        public void CreateNewOrder(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderModal createOrderModal = new CreateOrderModal(Client);
            createOrderModal.ShowDialog();
        }
    }
}
