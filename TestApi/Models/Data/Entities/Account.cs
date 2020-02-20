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
        public DateTime? CreateTime { get; set; }
        public string Image { get; set; }


        public Account()
        {
            CreateTime = DateTime.Now.Date;
        }
    }

}
