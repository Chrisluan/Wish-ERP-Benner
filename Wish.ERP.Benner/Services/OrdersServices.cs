using System;
using Wish.ERP.Benner.Models;

namespace Wish.ERP.Benner.Services
{
    public class OrdersServices
    {
        public static event Action OnOrdersChanged;
        public static void AddOrder(Order order)
        {
            DataManager.Instance.Orders.Add(order, PathTo.Order);
            OnOrdersChanged?.Invoke();
        }
        public static void Delete(string Id)
        {
            DataManager.Instance.Orders.DeleteById(Id, PathTo.Order);
            
            OnOrdersChanged?.Invoke();
        }
        public static void UpdateClient(Client client, Client newValue)
        {
            DataManager.Instance.Clients.UpdateById(client.Id, newValue, PathTo.Client);
            OnOrdersChanged?.Invoke();
        } 
    }
}
