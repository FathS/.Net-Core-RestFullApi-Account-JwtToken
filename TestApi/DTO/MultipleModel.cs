using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class MultipleModel
    {
        public List<AccountModel> accountModel { get; set; }
        public decimal TotalBakiye { get; set; }
    }
}
