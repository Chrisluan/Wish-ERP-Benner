using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Wish_ERP.Models;
using System.IO;
using System.Text.Json;
using System.ComponentModel;
using Wish_ERP.Services;
using System.Windows.Data;
namespace Wish_ERP.ViewModels
{
    public class ClientsViewModel : INotifyPropertyChanged
    {
        private List<Client> ClientList
        {
            get; set;
        }


        private ObservableCollection<Client> _filteredClientList;
        public ObservableCollection<Client> FilteredClientList
        {
            get => _filteredClientList;
            set
            {
                _filteredClientList = value;
                OnPropertyChanged(nameof(FilteredClientList));
            }
        }


        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if(_searchText != value)
                _searchText = value;
                ApplyFilter();
                
            }
        }

        public ClientsViewModel()
        {
            FilteredClientList = new ObservableCollection<Client>();
            _ = InitializeClients();
           
        }
        private async Task InitializeClients()
        {
            ClientList = await LoadClients();
            FilteredClientList = new ObservableCollection<Client>(ClientList);
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredClientList = new ObservableCollection<Client>(ClientList);
            }
            else
            {
                var filtered = ClientList
                    .Where(n => n.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || n.CPF.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

                FilteredClientList = new ObservableCollection<Client>(filtered);
            }
        }


        public async Task<List<Client>> LoadClients()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Clients.json");
                Console.WriteLine(DataManager.Instance.Clients);
                return DataManager.Instance.Clients;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ClientList;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}