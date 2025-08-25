using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Wish.ERP.Benner.Models;

namespace Wish.ERP.Benner.Services
{
    public class Repository<T>
    {
        private List<T> _items;

        public Repository(List<T> items)
        {
            _items = items ?? new List<T>();
        }

        public List<T> GetAll(Func<T, bool> predicate = null)
        {
            if (predicate != null)
            {
                return _items.Where(predicate).ToList();
            }
            return _items;
        }

        public T GetBy(Func<T, bool> predicate)
        {
            return _items.FirstOrDefault(predicate);
        }

        public bool Add(T item, PathTo type)
        {
            _items.Add(item);
            return JsonStorage.Include(item, type);
        }

        public bool UpdateById(string id, T newValue, PathTo type)
        {

            var obj = _items.FirstOrDefault(_obj =>
            {
                var prop = _obj.GetType().GetProperty("Id");
                if (prop == null) return false;
                string value = prop.GetValue(_obj)?.ToString();
                return value == id;
            });

            if (obj == null) return false;

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name == "Id") continue;

                var newVal = prop.GetValue(newValue);
                if (!prop.CanWrite) continue;
                prop.SetValue(obj, newVal);
            }


            JsonStorage.ModifyFieldById(type, id, newValue);

            return true;
        }


        public bool DeleteById(string Id, PathTo type)
        {
            var obj = _items.FirstOrDefault(_obj =>
            {
                var prop = _obj.GetType().GetProperty("Id");
                if (prop == null) return false;
                string value = prop.GetValue(_obj)?.ToString();
                return value == Id;
            });

            if (obj == null) return false;
            Debug.WriteLine(Id);
            if (JsonStorage.DeleteById(type, Id))
            {
                _items.Remove(obj);
                return true;
            }
            return false;
        }
    }

    class DataManager
    {
        public Repository<Client> Clients { get; private set; }
        public Repository<Product> Products { get; private set; }
        public Repository<Order> Orders { get; private set; }
        public Repository<List<string>> PaymentMethods { get; private set; }

        private static DataManager instance { get; set; }
        public static DataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                return instance;
            }
        }
        public DataManager()
        {
            InitializeFetch();
        }

        private void InitializeFetch()
        {

            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            Clients = new Repository<Client>(JsonStorage.FetchData<List<Client>>(PathTo.Client) ?? new List<Client>());
            Products = new Repository<Product>(JsonStorage.FetchData<List<Product>>(PathTo.Product));
            Orders = new Repository<Order>(JsonStorage.FetchData<List<Order>>(PathTo.Order));
        }

    }
}
