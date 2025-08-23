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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
