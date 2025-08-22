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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;
using Wish_ERP.ViewModels;

namespace Wish_ERP.Views
{
    public partial class ClientsView : UserControl
    {
        private static ClientsView instance = null;

        public static ClientsView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientsView();
                }
                return instance;
            }
        }
        public ClientsView()
        {
            InitializeComponent();
        }

        private void OpenClient(object sender, MouseButtonEventArgs e)
        {
            if (ClientsList.SelectedItem is Client selectedClient)
            {
                ClientServices.Delete(selectedClient.Id);
            }
        }
    }
}
