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
using Wish.ERP.Benner.Utils;

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
            if (currentClient != null) ClientCombo.SelectedValue = currentClient.Id;
            this.DataContext = this;
            PaymentMethodsCombo.ItemsSource = Enum.GetValues(typeof(PaymentMethod));
        }


        private void Done(object sender, RoutedEventArgs e)
        {
            var fields = new Dictionary<object, object>()
            {
                ["Cliente"] = ClientCombo,
            };
            var invalidFields = new List<object>();

            if (!Validator.IsFieldsFilled(fields, out invalidFields))
            {
                MessageBox.Show($"Preencha todos os Campos: {string.Join(" ,", invalidFields.Select(c => c.ToString()))}", "Validação", MessageBoxButton.OK, MessageBoxImage.Stop); return;
            }
            string _clientId = ClientCombo.SelectedValue.ToString();
            string _clientName = DataManager.Instance.Clients.GetBy(c => c.Id == _clientId).Name;

            Order order = new Order(Guid.NewGuid().ToString(), _clientId, _clientName, OrderBoxes.ToList(), (OrderStatus)StatusCombo.SelectedValue, DateTime.Now, (PaymentMethod)PaymentMethodsCombo.SelectedValue);

            CreatedOrder = order;

            bool success = OrdersServices.AddOrder(order);

            if (success)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Erro ao criar o pedido!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void ValidateForm(object sender, EventArgs e)
        {
            bool isValid = ClientCombo.SelectedItem != null
                           && OrderItemsList.Items.Count > 0
                           && StatusCombo.SelectedItem != null
                           && PaymentMethodsCombo.SelectedItem != null;

            FinishButton.IsEnabled = isValid;
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
            
            int quantity = 0;
            if (!int.TryParse(QuantityBox.Text, out quantity) || quantity <= 0)
            {
                QuantityBox.BorderBrush = Brushes.Red;
                return;
            }
            if (ProductCombo.SelectedValue != null)
            {
                Product product = DataManager.Instance.Products.GetBy(p => p.Id == ProductCombo.SelectedValue.ToString()) ?? null;
                if (product == null)
                {
                    ProductCombo.BorderBrush = Brushes.Red;
                    return;
                }
                ProductCombo.BorderBrush = Brushes.Gray;
                OrderBox order = new OrderBox()
                {
                    Product = product,
                    Amount = int.Parse(QuantityBox.Text)
                };
                OrderBoxes.Add(order);
            }
            ValidateForm(this, new EventArgs());
        }

        private void ProductCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
