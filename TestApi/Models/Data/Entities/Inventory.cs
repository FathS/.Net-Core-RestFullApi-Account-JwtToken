using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Marka { get; set; }
        public string SeriNo { get; set; }
        public string Feature { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual Account Account { get; set; }

        public Inventory()
        {
            CreateTime = DateTime.Now;
        }
    }
}
