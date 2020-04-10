using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class ProductModel
    {
        public string name { get; set; }
        public string size { get; set; }
        public decimal price { get; set; }
        public string amount { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public IFormFile file { get; set; }

        public Guid categoryid { get; set; }
    }
}
