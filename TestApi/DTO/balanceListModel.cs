using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class balanceListModel
    {
        public string user { get; set; }
        public decimal buyUsd { get; set; }
        public decimal buyTl{ get; set; }
        public decimal sellTl { get; set; }
        public decimal sellUsd { get; set; }
        public decimal selleuro { get; set; }
        public string date { get; set; }
        public decimal buyEuro { get; set; }
        public decimal euroKur { get; set; }
        public decimal dolarKur { get; set; }
    }
}
