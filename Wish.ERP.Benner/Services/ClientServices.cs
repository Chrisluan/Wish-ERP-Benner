using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wish.ERP.Benner.Models;

namespace Wish.ERP.Benner.Services
{
    public class ClientServices
    {
        public static event Action OnClientsChanged;


        public static void AddClient(Client client)
        {
            DataManager.Instance.Clients.Add(client, PathTo.Client);
            OnClientsChanged?.Invoke();
        }
        public static void Delete(string Id)
        {
            DataManager.Instance.Clients.DeleteById(Id, PathTo.Client);
            OnClientsChanged?.Invoke();
        }
        public static void DeleteMany(List<string> Ids)
        {
            foreach (var id in Ids)
            {
                DataManager.Instance.Clients.DeleteById(id, PathTo.Client);
            }
            OnClientsChanged?.Invoke();
        }
        public static void UpdateClient(string Id, Client newValue)
        {
            DataManager.Instance.Clients.UpdateById(Id, newValue, PathTo.Client);
            OnClientsChanged?.Invoke();
        } 
    }
}
