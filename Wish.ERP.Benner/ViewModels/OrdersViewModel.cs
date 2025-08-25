using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;

namespace Wish.ERP.Benner.ViewModels
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        public ICommand SetStatusCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private List<Order> selectedOrders;
        

        public List<Order> SelectedOrders
        {
            get => selectedOrders;
            set {
                if (selectedOrders != value)
                    selectedOrders = value;
                OnPropertyChanged(nameof(SelectedOrders));
            }
        }

        private List<Order> orders;

        private ObservableCollection<Order> filtered_orders;
        private decimal? filterMinValue = 0;
        private decimal? filterMaxValue = 0;
        private string searchText = string.Empty;

        public decimal? FilterMinValue
        {
            get => filterMinValue;
            set
            {
                if (filterMinValue != value)
                    filterMinValue = value;
                OnPropertyChanged(nameof(FilterMinValue));
                ApplyFilter();
            }
        }
        public decimal? FilterMaxValue
        {
            get => filterMaxValue;
            set
            {
                if (filterMaxValue != value && value is decimal)
                    filterMaxValue = value;
                OnPropertyChanged(nameof(FilterMaxValue));
                ApplyFilter();
            }
        }
        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText != value)
                    searchText = value;
                ApplyFilter();
            }
        }
        public ObservableCollection<Order> FilteredOrders
        {
            get => filtered_orders;
            set
            {
                filtered_orders = value;
                OnPropertyChanged(nameof(FilteredOrders));
            }
        }

        public OrdersViewModel()
        {
            filtered_orders = new ObservableCollection<Order>();
            
            UpdateOrdersList();
        }
        public void UpdateOrdersList()
        {
            orders = DataManager.Instance.Orders.GetAll();
            orders.Reverse();
            FilteredOrders = new ObservableCollection<Order>(orders);

            OrdersServices.OnOrdersChanged += UpdateOrdersList;

        }
        private void ApplyFilter()
        {
            var filtered = orders.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(n =>
                    n.ProductsNames.ToString().Contains(SearchText.ToLower())
                    || n.OrderBoxes.Any(e => e.Product.Name.ToLower().Contains(SearchText.ToLower()))
                );
            }

            if (FilterMinValue.HasValue && FilterMinValue.Value != 0)
                filtered = filtered.Where(n => n.TotalOrderPrice >= (double)FilterMinValue.Value);

            if (FilterMaxValue.HasValue && FilterMaxValue.Value != 0)
                filtered = filtered.Where(n => n.TotalOrderPrice <= (double)FilterMaxValue.Value);

            FilteredOrders = new ObservableCollection<Order>(filtered);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
