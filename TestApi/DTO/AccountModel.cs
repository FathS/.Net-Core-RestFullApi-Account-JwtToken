using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class AccountModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string email { get; set; }
        public bool isActive { get; set; }
        public DateTime createTime { get; set; }
        public string role { get; set; }
        public string age { get; set; }
        public string image { get; set; }
    }
}
