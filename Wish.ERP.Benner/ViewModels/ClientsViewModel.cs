using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Windows.Data;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;
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
            UpdateClientList();
        }
        public void UpdateClientList()
        {
            ClientList = DataManager.Instance.Clients.GetAll();
            ClientList.Reverse();
            FilteredClientList = new ObservableCollection<Client>(ClientList);

            ClientServices.OnClientsChanged += UpdateClientList;
            
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
                    .Where(n => n.Name.ToLower().Contains(SearchText.ToLower()) || n.CPF.Contains(SearchText));

                FilteredClientList = new ObservableCollection<Client>(filtered);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}