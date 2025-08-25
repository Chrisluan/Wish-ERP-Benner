
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wish.ERP.Benner.Models
{
    public enum OrderStatus
    {
        Pendente,
        Pago,
        Entregue,
        Enviado
    }
    public enum PaymentMethod
    {
        Dinheiro,
        Cartao,
        Boleto

    }
    public class Order
    {
        public string Id { get; }
        public string ClientId { get; set; }
        public List<OrderBox> OrderBoxes { get; set; }
        public double TotalOrderPrice => OrderBoxes?.Sum(p => p.Balance) ?? 0;
        public DateTime SaleDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public string ProductsNames
        {
            get
            {
                if (OrderBoxes == null || !OrderBoxes.Any())
                    return string.Empty;

                return string.Join(", ", OrderBoxes.Select(product => product.ProductName));
            }
        }
        public Order()
        {
            Id = Guid.NewGuid().ToString();
        }
        

        public OrderStatus Status { get; set; } = OrderStatus.Pendente;


    }
}