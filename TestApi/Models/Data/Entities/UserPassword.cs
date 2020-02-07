using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class UserPassword
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime CreatedPassword { get; set; }
        public DateTime ChangedPassword { get; set; }

        public UserPassword()
        {
            CreatedPassword = DateTime.Now;
        }
    }
}
