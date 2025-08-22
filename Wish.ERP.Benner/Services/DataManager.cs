using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Wish.ERP.Benner.Models;

namespace Wish.ERP.Benner.Services
{
    public class Repository<T> where T : class
    {
        private List<T> _items;

        public Repository(List<T> items)
        {
            _items = items ?? new List<T>();
        }

        public List<T> GetAll()
        {
            return _items;
        }

        public T GetBy(Func<T, bool> predicate)
        {
            return _items.FirstOrDefault(predicate);
        }

        public void Add(T item, PathTo type)
        {
            _items.Add(item);
            DataProvider.Include(item, type);
        }

        public bool Update(Func<T, bool> predicate, T newValue)
        {
            var index = _items.FindIndex(x => predicate(x));
            if (index == -1) return false;

            _items[index] = newValue;
            return true;
        }

        public bool Delete(Func<T, bool> predicate)
        {
            var item = _items.FirstOrDefault(predicate);
            if (item == null) return false;

            _items.Remove(item);
            return true;
        }
    }

    class DataManager
    {

        public Repository<Client> Clients { get; private set; }
        public Repository<Product> Products { get; private set; }
        public Repository<Order> Orders { get; private set; }
        public Repository<List<string>> PaymentMethods { get;private set; }

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
            Clients = new Repository<Client>(DataProvider.FetchData<List<Client>>(PathTo.Client) ?? new List<Client>());
            Products = new Repository<Product> (DataProvider.FetchData<List<Product>>(PathTo.Product));
            Orders = new Repository<Order>(DataProvider.FetchData<List<Order>>(PathTo.Order));
            Console.WriteLine(Clients == null ? "Clients é null" : $"Qtd clientes: {Clients.GetAll().Count}");
        }

    }
}
