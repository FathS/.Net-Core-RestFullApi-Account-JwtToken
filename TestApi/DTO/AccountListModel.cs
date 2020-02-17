using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class AccountListModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public bool isActive { get; set; }
        public DateTime createTime { get; set; }
        public string role { get; set; }
    }
}
