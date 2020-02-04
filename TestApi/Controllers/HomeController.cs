using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestApi.DTO;
using TestApi.Models.Data;
using TestApi.Models.Data.Entities;

namespace TestApi.Controllers
{

    public class HomeController : Controller
    {
        private readonly PersonelContext _db;
        private readonly Users _user;
        public HomeController(PersonelContext db, Users user)
        {
            _db = db;
            _user = user;
        }


        public IActionResult CityList()
        {
            //var cityList = _db.Set<City>().ToList();

            var cities = _db.Set<City>().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,

            });

            return Json(cities);
        }
        public IActionResult managerList()
        {
            //var cityList = _db.Set<City>().ToList();

            var manager = _db.Set<Manager>().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FullName,

            });

            return Json(manager);
        }
        public IActionResult Index()
        {
            //Users user = new Users
            //{
            //    Id = 1,
            //    Name = "Fatih",
            //    Surname = "Şengül",
            //    Department = "Yazılım",
            //};

            //List<Users> ulist = new List<Users>();

            //ulist.Add(user);

            //Üstteki kod DB oluşturmadan test etmek için.//


            var uList = _db.Set<Users>().Select(x => new usersDTO
            {
                id = x.Id,
                name = x.Name,
                surname = x.Surname,
                manager = x.Manager.FullName,
                city = x.City.Name,
                IsActive = x.IsActive
            }).OrderBy(x => x.name).ToList();


            return Ok(uList);
        }
        public IActionResult Get(int id)
        {
            var getUser = _db.Set<Users>().Find(id);
            return Json(getUser);
        }

        [HttpPost]
        public IActionResult Add([FromBody] usersDTO user)
        {
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "İsimsiz misin?";
                //Hata Verdirilebilir.
            }
            //Devam Edilebilir...

            Users users = new Users
            {
                Name = user.name,
                Surname = user.surname,
                CityId = user.cityId,
                ManagerId = user.managerId,
                Birthday = Convert.ToDateTime(user.birthday),
                Gender = user.gender,
                Department = user.department,
            };

            _db.Entry(users).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return Json(user);
        }
        [HttpPut]
        public IActionResult Update([FromBody] Users user)
        {
            _db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return Json(user);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Users user = _db.Set<Users>().Find(id);
            _db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _db.SaveChanges();
            return Ok(user);
        }



        [HttpPost]
        public IActionResult Register([FromBody] Account model)
        {
            var test = _db.Set<Account>().FirstOrDefault(x => x.Email == model.Email);

            if (test != null)
            {
                if (test.Email == model.Email)
                {
                    return BadRequest();
                }
            }

            if (ModelState.IsValid)
            {
                if (model.Password == model.ConfirPassword)
                {
                    _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _db.SaveChanges();
                    return Json(model);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Login([FromBody] loginModel model)
        {
            if (ModelState.IsValid)
            {
                var login = _db.Set<Account>().FirstOrDefault(x => x.Email == model.email && x.Password == model.password);
                

                if (login == null)
                {
                    return BadRequest();
                }
                return Json(login);
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult ChangePassword([FromBody] changePassModel model)
        {
            var ps = _db.Set<Account>().FirstOrDefault(x => x.Id == model.id);

            if (ps.Password == model.oldPassword)
            {
                ps.Password = model.password;
                ps.ConfirPassword = model.confirPassword;
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }
    }
}
