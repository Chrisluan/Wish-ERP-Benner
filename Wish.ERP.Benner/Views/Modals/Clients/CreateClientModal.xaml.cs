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
using System.Windows.Shapes;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;

namespace Wish.ERP.Benner.Views.Modals.Clients
{
    /// <summary>
    /// Lógica interna para CreateClientModal.xaml
    /// </summary>
    public partial class CreateClientModal : Window
    {
        public CreateClientModal()
        {
            InitializeComponent();
            var mainw = Application.Current.MainWindow;

            mainw.Effect = new System.Windows.Media.Effects.BlurEffect
            {
                Radius = 5
            };
        }

        private void Done(object sender, RoutedEventArgs e)
        {

            var fields = new Dictionary<object, TextBox>()
            {
                ["name"] = NameBox,
                ["address"] = AddresBox,
                ["CPF"] = CPFBox
            };

            foreach (var field in fields)
            {
                if (field.Value != null)
                    field.Value.BorderBrush = Brushes.Gray;
            }

            var emptyFields = fields.ToList().Where(v => v.Value.Text.Equals(string.Empty));

            foreach (var field in emptyFields)
            {
                if (field.Value != null)
                    field.Value.BorderBrush = Brushes.Red;
            }

            if (emptyFields.Where(f => f.Key.ToString() != "address").Any()) return;

            if (!Validator.IsCPFValid(fields["CPF"].Text))
            {
                MessageBox.Show("CPF INVÁLIDO", "Validação", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            Client client = new Client(fields["name"].Text, fields["CPF"].Text, fields["address"].Text ?? "");
            ClientServices.AddClient(client);
            this.Close();


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
    }
}
