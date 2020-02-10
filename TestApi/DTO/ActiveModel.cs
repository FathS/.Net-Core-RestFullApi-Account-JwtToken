using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class ActiveModel
    {
        public string email { get; set; }
        public bool active { get; set; }
        public string password { get; set; }
    }
}
