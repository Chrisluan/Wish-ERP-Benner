using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wish_ERP.Models;

namespace Wish_ERP.Services
{
    class DataManager
    {

        public List<Client> Clients { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Order> Orders { get; private set; }

        private static DataManager instance { get; set; }
        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance =  new DataManager();
                }
                return instance;
            }
        }
        public DataManager() {
            InitializeFetch();
            Console.WriteLine("DataManager initialized.");
        }

        private void InitializeFetch()
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            Clients =  DataProvider.FetchData<List<Client>>(Path.Combine(basePath, "Clients.json"));
            Products =  DataProvider.FetchData<List<Product>>(Path.Combine(basePath, "Product.json"));
            Orders =  DataProvider.FetchData<List<Order>>(Path.Combine(basePath, "Orders.json"));
            Console.WriteLine(Clients == null ? "Clients é null" : $"Qtd clientes: {Clients.Count}");
        }

    }
}
