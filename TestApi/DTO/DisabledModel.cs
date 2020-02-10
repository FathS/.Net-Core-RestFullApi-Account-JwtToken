using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.DTO
{
    public class DisabledModel
    {
        public Guid id { get; set; }
        public bool isActive { get; set; }
    }
}
