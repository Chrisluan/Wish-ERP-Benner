using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wish.ERP.Benner.Models;
using Wish.ERP.Benner.Services;

namespace Wish.ERP.Benner.ViewModels
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
                string lowerSearch = SearchText.ToLower();

                filtered = filtered.Where(n =>
                    (n.ProductsNames?.ToString().ToLower().Contains(lowerSearch) ?? false)
                    || (n.OrderBoxes?.Any(e => e.Product?.Name?.ToLower().Contains(lowerSearch) ?? false) ?? false)
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
