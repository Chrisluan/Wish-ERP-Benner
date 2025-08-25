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
    /// Lógica interna para ClientOverview.xaml
    /// </summary>
    public partial class EditClientModal : Window
    {

        private Client editingClient;
        public EditClientModal(Client client)
        {
            InitializeComponent();
            editingClient = client;
            NameBox.Text = editingClient.Name;
            CPFBox.Text = editingClient.CPF;
            AddresBox.Text = editingClient.Address;

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

            if (emptyFields.Any() || !CPFValidator(fields["CPF"].Text)) return;


            Client client = new Client(fields["name"].Text, fields["CPF"].Text, fields["address"].Text);


            ClientServices.UpdateClient(editingClient.Id, client);
            this.Close();
        }
        private void AllowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        static bool CPFValidator(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11)
                return false;
            if (cpf.Distinct().Count() == 1)
                return false;
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (cpf[i] - '0') * (10 - i);

            int remainder = sum % 11;
            int firstDigit = (remainder < 2) ? 0 : 11 - remainder;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (cpf[i] - '0') * (11 - i);

            remainder = sum % 11;
            int secondDigit = (remainder < 2) ? 0 : 11 - remainder;
            return (cpf[9] - '0' == firstDigit) && (cpf[10] - '0' == secondDigit);
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
