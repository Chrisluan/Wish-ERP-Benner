
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wish.ERP.Benner.Models
{
    public enum OrderStatus
    {
        Pendente,
        Pago,
        Enviado,
        Recebido
    }
    public enum PaymentMethod
    {
        Dinheiro,
        Cartao,
        Boleto

    }
    public class Order 
    {
        private Guid guid;
        private object clientId;
        private object value;

        public string Id { get; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
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
        
        public OrderStatus Status { get; set; } = OrderStatus.Pendente;

        public Order(string id, string clientId, string clientName, List<OrderBox> orderBoxes, OrderStatus status, DateTime saleDate, PaymentMethod paymentMethod)
        {
            
            Id = id;
            ClientId = clientId;
            ClientName = clientName;
            OrderBoxes = orderBoxes;
            Status = status;
            SaleDate = saleDate;
            PaymentMethod = paymentMethod;

        }
    }
}