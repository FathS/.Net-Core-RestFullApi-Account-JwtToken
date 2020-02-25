using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Models.Data.Entities
{
    public class Balance
    {
        public int Id { get; set; }
        public Guid? AccountId { get; set; }
        public decimal BuyUSD { get; set; }
        public decimal BuyTL { get; set; }
        public decimal SellTL { get; set; }
        public decimal SellUSD { get; set; }
        public decimal SellEURO { get; set; }
        public string OperationTime { get; set; }
        public decimal DolarKur { get; set; }
        public decimal EuroKur { get; set; }
        public virtual Account Account { get; set; }

        
    }
}
