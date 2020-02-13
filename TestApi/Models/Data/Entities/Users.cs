
using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Models.Data.Entities
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Department { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public int? ManagerId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public virtual District District { get; set; }
        public virtual City City { get; set; }
        public virtual Manager Manager { get; set; }
        
    }
}
