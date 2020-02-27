using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class dovizModel
    {
        public string dolar { get; set; }
        public string euro { get; set; }
        public string dolarAlis { get; set; }
        public string euroAlis { get; set; }
        public decimal altin { get; set; }
        public string altinAlis { get; set; }
        public DateTime date { get; set; }
        public DateTimeOffset updatekur { get; set; }

    }
}
