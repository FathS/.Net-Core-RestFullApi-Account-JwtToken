using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class InventoryListModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public string marka { get; set; }
        public string seriNo { get; set; }
        public string feature { get; set; }
        public bool status { get; set; }
        public DateTime createTime { get; set; }
        public string accountMail { get; set; }
    }
}
