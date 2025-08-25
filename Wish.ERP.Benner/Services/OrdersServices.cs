using System;
using System.Collections.Generic;
using System.Linq;
using Wish.ERP.Benner.Models;

namespace Wish.ERP.Benner.Services
{
    public class OrdersServices
    {
        public static event Action OnOrdersChanged;
        public static bool AddOrder(Order order)
        {
            bool result = false;
            result = DataManager.Instance.Orders.Add(order, PathTo.Order);
            if (!result) return false;

            OnOrdersChanged?.Invoke();
            return result;
        }
        public static void DeleteMany(List<string> Ids)
        {
            foreach (var id in Ids)
            {
                DataManager.Instance.Orders.DeleteById(id, PathTo.Order);
            }
            OnOrdersChanged?.Invoke();
        }
        public static void Delete(string Id)
        {
            DataManager.Instance.Orders.DeleteById(Id, PathTo.Order);
            OnOrdersChanged?.Invoke();
        }
        public static void UpdateClient(string orderId, Order newValue)
        {
            DataManager.Instance.Orders.UpdateById(orderId, newValue, PathTo.Client);
            OnOrdersChanged?.Invoke();
        } 
    }
}
