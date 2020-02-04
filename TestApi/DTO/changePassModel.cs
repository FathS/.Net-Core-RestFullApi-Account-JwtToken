using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class changePassModel
    {
        public int id { get; set; }
        public string oldPassword { get; set; }
        public string password { get; set; }
        public string confirPassword { get; set; }
    }
}
