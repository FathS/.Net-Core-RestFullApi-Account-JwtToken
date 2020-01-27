using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class usersDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string manager { get; set; }
        public string city { get; set; }
        public int cityId { get; set; }
        public int managerId { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string department { get; set; }
        public bool? IsActive { get; set; }
    }
}
