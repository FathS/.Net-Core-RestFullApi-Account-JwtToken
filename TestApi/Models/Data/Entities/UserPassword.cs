using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class UserPassword
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime CreatedPassword { get; set; }
        public bool ActivePassword { get; set; }

        public UserPassword()
        {
            CreatedPassword = DateTime.Now;
        }
    }
}
