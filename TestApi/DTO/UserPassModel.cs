using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class UserPassModel
    {
        public int userId { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}
