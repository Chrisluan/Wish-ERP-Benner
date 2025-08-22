
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wish.ERP.Benner.Models
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Delivered,
        Sent
    }
    public class Order
    {
        public string Id { get; set; }
        public Client Client { get; set; }
        public List<Product> Products { get; set; }
        public double TotalPrice 
        { 
            get 
            {
                return Products.Sum(p => p.Price);
            }
        }
        public DateTime SaleDate { get; set; }

        public string Address { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        public Order(Client client, List<Product> products, DateTime saleDate)
        {
            Id = Guid.NewGuid().ToString();
            this.Client = client;
            this.SaleDate = saleDate;
            this.Products = products;
            SaleDate = saleDate;
        }
    }
}