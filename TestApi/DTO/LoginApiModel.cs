using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class LoginApiModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string token { get; set; }
    }
}
