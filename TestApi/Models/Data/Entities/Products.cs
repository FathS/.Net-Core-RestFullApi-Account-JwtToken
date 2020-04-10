using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class Products
    {
        [Key]
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public string Amount { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
