using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wish.ERP.Benner.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public double Price { get; set; }

        public Product(string name, int code, double price)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.Code = code;
            this.Price = price;
        }
    }
}
