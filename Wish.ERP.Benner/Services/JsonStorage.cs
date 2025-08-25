using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    public static class JsonStorage
    {
        private static string GetDataPath(PathTo type)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            switch (type)
            {
                case PathTo.Client:
                    return Path.Combine(basePath, "Clients.json");
                case PathTo.Product:
                    return Path.Combine(basePath, "Products.json");
                case PathTo.Order:
                    return Path.Combine(basePath, "Orders.json");
                case PathTo.PaymentMethods:
                    return Path.Combine(basePath, "PaymentMethods.json");
                default:
                    throw new ArgumentException("Tipo de dado desconhecido", nameof(type));
            }
        }

        public static T FetchData<T>(PathTo type)
        {
            try
            {
                var path = GetDataPath(type);
                if (!File.Exists(path)) File.WriteAllText(path, "[]");
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                throw;
            }
        }

        public static bool Include<T>(T item, PathTo type)
        {
            try
            {
                var path = GetDataPath(type);
                var json = File.Exists(path) ? File.ReadAllText(path) : "[]";
                var array = JArray.Parse(json);
                var newItem = JObject.FromObject(item);
                array.Add(newItem);
                File.WriteAllText(path, array.ToString(Formatting.Indented));
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
                var jArray = JArray.Parse(jsonFile);

                var itemToRemove = jArray.FirstOrDefault(obj =>
                    obj["Id"] != null && obj["Id"].ToString().Equals(id, StringComparison.OrdinalIgnoreCase)
                );

                if (itemToRemove != null)
                {
                    jArray.Remove(itemToRemove);
                    File.WriteAllText(path, jArray.ToString(), Encoding.UTF8);
                    return true;
                }

                Console.WriteLine($"Item com o Id '{id}' não encontrado");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
                return false;
            }
        }
        public static bool ModifyFieldById(PathTo type, string id, object newValue)
        {
            try
            {
                var path = GetDataPath(type);
                var json = File.Exists(path) ? File.ReadAllText(path) : "[]";
                var array = JArray.Parse(json);

                var item = array.FirstOrDefault(d => d["Id"]?.ToString()?.Trim().ToLower() == id.Trim().ToLower());
                if (item == null) return false;

                var newValues = JObject.FromObject(newValue);
                foreach (var prop in newValues.Properties())
                {
                    if (prop.Name == "Id") continue; // não sobrescreve Id
                    item[prop.Name] = prop.Value;
                }

                File.WriteAllText(path, array.ToString(Formatting.Indented));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying item: {ex.Message}");
                return false;
            }
        }
    }
}
