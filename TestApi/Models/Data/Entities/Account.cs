using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public bool? isActive { get; set; }
        public string Role { get; set; }
        public string CreateTime { get; set; }
        public string Image { get; set; }
        public ICollection<Inventory> Inventory { get; set; }
        public ICollection<Balance> Balance { get; set; }
        public decimal TL { get; set; }
        public decimal USD { get; set; }
        public decimal EURO { get; set; }
        public decimal AltinGr { get; set; }

        public Account()
        {
            CreateTime = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString();
        }
    }

}
