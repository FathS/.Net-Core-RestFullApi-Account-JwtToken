using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTO;
using TestApi.Models.Data;
using TestApi.Models.Data.Entities;

namespace TestApi.Controllers
{
    public class AdminController : Controller
    {
        private readonly PersonelContext _db;
        public AdminController(PersonelContext db)
        {
            _db = db;
        }
        public IActionResult AccountList()
        {
            var list = _db.Set<Account>().Select(x => new AccountModel
            {
                id = x.Id,
                name = x.Name,
                surname = x.Surname,
                email = x.Email,
                isActive = (bool)x.isActive,
                createTime = (DateTime)x.CreateTime,
                role = x.Role
            }).ToList();

            return Json(list);
        }

        public IActionResult DetailAccount(Guid id)
        {
            var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);
            if (account == null)
            {
                return BadRequest("Hesap Bulunamadı!");
            }


            return Ok(account);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var account = _db.Set<Account>().Find(id);

            if (account == null)
            {
                return BadRequest("Hesap Bulunamadı");
            }
            _db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _db.SaveChanges();
            return Ok("Hesap Silindi!");
        }
        [HttpPost]
        public IActionResult DisabledAccount(Guid id)
        {
            var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);

            if (account == null)
            {
                return BadRequest("Hesap Bulunamadı");
            }
            else
            {
                account.isActive = false;
                _db.SaveChanges();
                return Ok("Hesap Devre Dışı Bırakıldı");
            }

        }
    }
}