using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;
using Wish.ERP.Benner.Views.Modals.Clients;
using Wish_ERP.ViewModels;

namespace Wish_ERP.Views
{
    public partial class ClientsView : UserControl
    {

        private static ClientsView instance = null;

        public event PropertyChangedEventHandler PropertyChanged;


        private List<Client> selectedClients;
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

        private void AddClient(object sender, RoutedEventArgs e)
        {
            Client newClient = new Client("", "", "");

        }
        private void DeleteClient(object sender, RoutedEventArgs e)
        {

           
            var selectedClients = ClientsList.SelectedItems.Cast<Client>().ToList();
            if (selectedClients.Count == 1)
            {
                MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja excluir {selectedClients[0].Name}? Essa ação é Irreversível", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;
                ClientServices.Delete(selectedClients[0].Id);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"Tem certeza que deseja excluir esses {selectedClients.Count} clientes? Essa ação é Irreversível", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(result == MessageBoxResult.No) return;
                var ids = selectedClients.Select(c => c.Id).ToList();
                ClientServices.DeleteMany(ids);
            }
        }
        private void OpenClient(object sender, MouseButtonEventArgs e)
        {
            if (ClientsList.SelectedItem is Client selectedClient)
            {

            }
        }

        private void HandleSelectionList(object sender, SelectionChangedEventArgs e)
        {
            selectedClients = ClientsList.SelectedItems.Cast<Client>().ToList();
            DeleteClientButton.IsEnabled = selectedClients.Any();
        }

        private void CreateClient(object sender, RoutedEventArgs e)
        {
            var CreateClientModal = new CreateClientModal();
            CreateClientModal.ShowDialog();
            
        }
    }
}
