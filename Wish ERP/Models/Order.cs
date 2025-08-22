
namespace Wish_ERP.Models


{
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

        public string ?Address { get; set; }

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