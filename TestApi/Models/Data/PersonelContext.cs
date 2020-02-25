using Microsoft.EntityFrameworkCore;
using TestApi.Models.Data.Entities;

namespace TestApi.Models.Data
{
    public class PersonelContext : DbContext
    {
        DbSet<Users> Users { get; set; }
        DbSet<City> City { get; set; }
        DbSet<District> District { get; set; }
        DbSet<Manager> Manager { get; set; }
        DbSet<Account> Account { get; set; }
        DbSet<UserPassword> UserPassword { get; set; }
        DbSet<Balance> Balance { get; set; }


        public PersonelContext(DbContextOptions<PersonelContext> option) : base(option)
        {

        }

    }
}
