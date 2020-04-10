using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}
