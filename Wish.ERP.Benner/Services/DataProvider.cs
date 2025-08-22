using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Wish.ERP.Benner.Models;

namespace Wish.ERP.Benner.Services
{
    public enum DataType
    {
        Client,
        Product,
        Order
    }
    public class DataProvider
    {
        private static string GetDataPath(DataType type)
        {
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            switch(type)
            {
                case DataType.Client:
                    return Path.Combine(basePath, "Clients.json");
                case DataType.Order:
                    return Path.Combine(basePath, "Orders.json");
                case DataType.Product:
                    return Path.Combine(basePath, "Products.json");
                default:
                    throw new ArgumentException("Tipo de dado desconhecido", nameof(type));
            }
            ;
        }
        public static T FetchData<T>(DataType expectedType)
        {
            try
            {
                var path= GetDataPath(expectedType);
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
        public static bool ModifyFieldById(DataType expectedType, string id, string field, object newValue)
        {
            var path = GetDataPath(expectedType);
            var jsonFile = File.ReadAllText(path);

            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonFile);

            var item = data.FirstOrDefault(d => d["Id"].ToString().ToLower().Equals(id.ToLower()));
            if (item.ContainsKey(field))
            {
                if (field == "Id")
                    throw new Exception("Não é possível editar o campo ID");

                item[field] = newValue;

                File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented), Encoding.UTF8);

                Console.WriteLine($"Field '{field}' of item with ID '{id}' has been updated to '{newValue}'.");
                return true;
            }
            else
            {
                Console.WriteLine($"Item com o Id '{id}' não encontrado ou campo inexistente");
                return false;
            }
        }
        public static bool Include<T>(T item, DataType expectedType)
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

    }
}
