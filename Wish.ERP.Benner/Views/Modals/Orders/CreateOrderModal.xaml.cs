using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;

namespace Wish.ERP.Benner.Views.Modals.Orders
{
    public partial class CreateOrderModal : Window
    {

        public Order CreatedOrder { get; private set; }
        public ObservableCollection<OrderBox> OrderBoxes { get; set; } = new ObservableCollection<OrderBox>();

        public CreateOrderModal(Client currentClient = null)
        {
            InitializeComponent();
            var mainw = Application.Current.MainWindow;
            DataContext = this;
            mainw.Effect = new System.Windows.Media.Effects.BlurEffect
            {
                Radius = 5
            };
            StatusCombo.ItemsSource = Enum.GetValues(typeof(OrderStatus));
            ProductCombo.ItemsSource = DataManager.Instance.Products.GetAll();
            ClientCombo.ItemsSource = DataManager.Instance.Clients.GetAll();
            if(currentClient != null) ClientCombo.SelectedValue = currentClient.Id;
            PaymentMethodsCombo.ItemsSource = Enum.GetValues(typeof(PaymentMethod));
        }


        private void Done(object sender, RoutedEventArgs e)
        {
            string _clientId = ClientCombo.SelectedValue.ToString();

            Order order = new Order()
            {
                ClientId = _clientId,
                OrderBoxes = OrderBoxes.ToList(),
                SaleDate = DateTime.Now,
                Status = OrderStatus.Pendente,
                PaymentMethod = (PaymentMethod)PaymentMethodsCombo.SelectedValue
            };

            CreatedOrder = order;

            // Salva no banco/serviço
            bool sucesso = OrdersServices.AddOrder(order);

            if (sucesso)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Erro ao criar o pedido!");
            }
        }

        private void AllowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            var mainw = Application.Current.MainWindow;
            if (mainw != null)
            {
                mainw.Effect = null;
            }
        }

        private void AddProduct(object sender, RoutedEventArgs e)
        {

            Product product = DataManager.Instance.Products.GetBy(p => p.Id == ProductCombo.SelectedValue.ToString());
            OrderBox order = new OrderBox()
            {
                Product = product,
                Amount = int.Parse(QuantityBox.Text)
            };
            OrderBoxes.Add(order);

            foreach (var item in OrderBoxes)
            {
                MessageBox.Show((item.ProductName, item.Amount).ToString());
            }
        }
    }
}
