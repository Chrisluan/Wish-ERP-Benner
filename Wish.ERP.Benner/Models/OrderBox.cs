using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wish.ERP.Benner.Models
{
    public class OrderBox
    {
        public string Id { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }

        public double Balance => Product.Price * Amount;
       
        public string ProductName => string.Join(" ,", $"({Amount}x) {Product.Name}") ;
        public double ProductPrice => Product.Price;

    }
}
