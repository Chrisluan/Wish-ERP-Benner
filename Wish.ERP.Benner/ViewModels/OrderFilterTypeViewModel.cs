using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wish.ERP.Benner.Models;

namespace Wish.ERP.Benner.ViewModels
{
    public enum FilterOrderStatus
    {
        All,
        Paid,
        Delivered,
        Pendent,
        Sent
    }
    public class OrderFilterTypeViewModel:INotifyPropertyChanged
    {

        public FilterOrderStatus orderStatus = FilterOrderStatus.All;

        public event PropertyChangedEventHandler PropertyChanged;

        public FilterOrderStatus OrderStatus
        {
            get => orderStatus; set
            {
                if (orderStatus != value)
                    orderStatus = value;
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
