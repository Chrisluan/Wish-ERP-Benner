using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Wish_ERP.Models;

namespace Wish_ERP.Services
{
    public class DataProvider
    {
        public static T FetchData<T>(string path)
        {
            try
            {
                string jsonFile = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<T>(jsonFile);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return default;
            }

        }
        public static async Task<bool> ModifyFieldById(string path, string id, string field, object newValue)
        {
            var jsonFile = await File.ReadAllTextAsync(path);

            var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonFile);

            var item = data.FirstOrDefault(d => d["Id"].ToString().ToLower().Equals(id.ToLower()));
            if (item.ContainsKey(field))
            {
                if(field == "Id")
                    throw new Exception("Não é possível editar o campo ID");

                item[field] = newValue;
                
                await File.WriteAllTextAsync(path, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }), Encoding.UTF8);

                Console.WriteLine($"Field '{field}' of item with ID '{id}' has been updated to '{newValue}'.");
                return true;
            }
            else
            {
                Console.WriteLine($"Item com o Id '{id}' não encontrado ou campo inexistente");
                return false;
            }
        }

    }
}
