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
            }).OrderBy(x=> x.role).ToList();

            return Json(list);
        }
        public IActionResult CreatePassword(Guid UserId, string Password, string ConfirmPassword)
        {
            //Admin Kullanıcının şifresini değiştirirse eski parolayı false yapmak için yazıldı.
            var password = _db.Set<UserPassword>().FirstOrDefault(x => x.UserId == UserId && x.ActivePassword);
            password.ActivePassword = false;
            ////////////////////////////////

            if (ModelState.IsValid)
            {
                var pass = new UserPassword
                {
                    UserId = UserId,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword,
                    ActivePassword = true
                };
                if (Password == ConfirmPassword)
                {
                    _db.Entry(pass).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _db.SaveChanges();
                    return Json(pass);
                }
                return BadRequest("Şifreler Eşleşmedi");
            }

            return BadRequest("Parola Oluşturulamadı");
        }

        public IActionResult DetailAccount(Guid id)
        {
            var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);
            if (account == null)
            {
                return BadRequest("Hesap Bulunamadı!");
            }
            var password = _db.Set<UserPassword>().FirstOrDefault(x => x.UserId == id && x.ActivePassword);
            if (password == null)
            {
                return BadRequest("Parola Bulunamadı!");
            }

            var accountModel = new AccountModel
            {
                id = account.Id,
                isActive = (bool)account.isActive,
                name = account.Name,
                surname = account.Surname,
                email = account.Email,
                role = account.Role,
                password = password.Password,
                age = account.Age
            };

            return Ok(accountModel);
        }

        [HttpPost]
        public IActionResult UpdateAccount([FromBody]AccountModel model)
        {
            var account = new Account
            {
                Id = model.id,
                Name = model.name,
                Surname = model.surname,
                Email = model.email,
                Age = model.age,
                Role = model.role,
                isActive = model.isActive
            };

            if (model.password != null && model.confirmPassword != null)
            {
                CreatePassword(model.id, model.password, model.confirmPassword);
            }

            _db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            if (model.isActive)
            {
                return Ok("Kullanıcı Bilgileri Güncellendi" + " " + account.Name + " " + account.Surname + " " + "Aktifdir. ");
            }
            if (!model.isActive)
            {
                return Ok("Kullanıcı Bilgileri Güncellendi" + " " + account.Name + " " + account.Surname + " " + "Aktif Değildir. ");
            }
            return Ok("Kullanıcı Bilgileri Güncellendi.");
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
        public IActionResult AccountisActive(Guid id)
        {
            var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);

            if (account == null)
            {
                return BadRequest("Hesap Bulunamadı");
            }
            if ((bool)account.isActive)
            {
                account.isActive = false;
                _db.SaveChanges();
                return Ok(account.Name + " " + " " + account.Surname + "'e Ait Hesap Devre Dışı Bırakıldı");
            }
            else
            {
                account.isActive = true;
                _db.SaveChanges();
                return Ok(account.Name + " " + " " + account.Surname + "'e Ait Hesap Aktif Edildi");
            }

        }


    }
}