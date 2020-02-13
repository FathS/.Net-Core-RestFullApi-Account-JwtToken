using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class DistrictModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int cityId { get; set; }
    }
}
