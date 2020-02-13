using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class registerModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public string Password { get; set; }
        public string ConfirPassword { get; set; }
        public bool isActive { get; set; }
    }
}
