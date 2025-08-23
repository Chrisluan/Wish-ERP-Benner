using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wish.ERP.Benner.Services
{
    public enum PathTo
    {
        Client,
        Product,
        Order,
        PaymentMethods
    }
    public class JsonStorage
    {
        private static string GetDataPath(PathTo type)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            switch (type)
            {
                case PathTo.Client:
                    return Path.Combine(basePath, "Clients.json");
                case PathTo.Order:
                    return Path.Combine(basePath, "Orders.json");
                case PathTo.Product:
                    return Path.Combine(basePath, "Products.json");
                case PathTo.PaymentMethods:
                    return Path.Combine(basePath, "PaymentMethods.json");
                default:
                    throw new ArgumentException("Tipo de dado desconhecido", nameof(type));
            }
            ;
        }
        public static T FetchData<T>(PathTo expectedType)
        {
            try
            {
                var path = GetDataPath(expectedType);
                if(!File.Exists(path))
                {
                    File.WriteAllText(path, "[]", Encoding.UTF8);
                }
                string jsonFile = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<T>(jsonFile);
                return data;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                throw;
            }

        }
        public static bool ModifyFieldById(PathTo expectedType, string id, object newValue)
        {
            var path = GetDataPath(expectedType);
            var jsonFile = File.ReadAllText(path);

            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonFile)
                       ?? new List<Dictionary<string, object>>();

            var item = data.FirstOrDefault(d => d.ContainsKey("Id") && d["Id"].ToString().Equals(id, StringComparison.OrdinalIgnoreCase));

            if (item == null)
                return false;

            var newValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                JsonConvert.SerializeObject(newValue)
            );
            foreach (var _item in newValues)
            {
                if (_item.Key == "Id") continue;
                item[_item.Key] = _item.Value;
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented), Encoding.UTF8);

            return true;
        }

        public static bool Include<T>(T item, PathTo expectedType)
        {
            var path = GetDataPath(expectedType);
            try
            {
                var jsonFile = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<List<T>>(jsonFile) ?? new List<T>();
                data.Add(item);
                File.WriteAllText(path, JsonConvert.SerializeObject(data), Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error including item: {ex.Message}");
                return false;
            }
        }
        public static bool DeleteById(PathTo expectedType, string id)
        {
            var path = GetDataPath(expectedType);
            try
            {
                var jsonFile = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonFile) ?? new List<Dictionary<string, object>>();
                var itemToRemove = data.FirstOrDefault(d => d["Id"].ToString().ToLower().Equals(id.ToLower()));
                if (itemToRemove != null)
                {
                    data.Remove(itemToRemove);
                    File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented), Encoding.UTF8);
                    return true;
                }
                else
                {
                    Console.WriteLine($"Item com o Id '{id}' não encontrado");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
                return false;
            }
        }


    }
}
