
namespace Wish_ERP.Models


{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string Address { get; set; }

        public Client(string name, string cpf, string address)
        {
            Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.CPF = cpf;
            this.Address = address;
        }
    }
}