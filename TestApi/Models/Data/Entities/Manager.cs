using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class Manager
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
