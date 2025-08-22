
using System;

namespace Wish.ERP.Benner.Models
{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string Address { get; set; }

        public Client(string name, string cpf, string address)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = name;
            this.CPF = cpf;
            this.Address = address;
        }
    }
}