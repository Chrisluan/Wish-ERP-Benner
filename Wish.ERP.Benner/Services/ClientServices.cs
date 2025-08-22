using System;
using System.Collections.Generic;
using System.Linq;
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
            DataManager.Instance.Clients.Delete(x => x.Id == Id);
            DataProvider.DeleteById(PathTo.Client, Id);
            OnClientsChanged?.Invoke();
        }
        public static void EditClient()
        {

            OnClientsChanged?.Invoke();
        }
    }
}
