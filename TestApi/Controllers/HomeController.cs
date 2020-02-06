using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestApi.DTO;
using TestApi.Models.Data;
using TestApi.Models.Data.Entities;

namespace TestApi.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly PersonelContext _db;
        private readonly Users _user;
        private IConfiguration _config;
        public HomeController(PersonelContext db, Users user, IConfiguration config)
        {
            _db = db;
            _user = user;
            _config = config;
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
                    return BadRequest("Hatalı İşlem");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.Password == model.ConfirPassword)
                {
                    _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _db.SaveChanges();
                    return Json("Kayıt İşlemi Başarılı");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Login([FromBody] loginModel model)
        {
            //IActionResult response = Unauthorized();
            if (ModelState.IsValid)
            {
                var login = _db.Set<Account>().FirstOrDefault(x => x.Email == model.email && x.Password == model.password);

                if (login == null)
                {
                    return BadRequest("Kullanıcı adı veya şifre hatalı");
                }

                var tokenString = GenerateJSONWebToken(model);

                var loginApi = new LoginApiModel
                {
                    id = login.Id,
                    name = login.Name,
                    surname = login.Surname,
                    token = tokenString
                };

                //response = Ok(new { token = tokenString });

                return Json(loginApi);
            }
            return BadRequest();
        }
        //private string GetToken(loginModel model)
        //{
        //    Claim[] claims;

        //    claims = new[]
        //    {
        //            new Claim(JwtRegisteredClaimNames.Sub, model.email),
        //            new Claim(JwtRegisteredClaimNames.Sub, model.password)
        //    };

        //    var key = new SymmetricSecurityKey(Convert.FromBase64String(_config["Authentication:JwtKey"]));

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddDays(1),//1 day will be valid.
        //        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        //    );
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        private string GenerateJSONWebToken(loginModel model)
        {
            Claim[] claims;

            claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, model.email),
                    new Claim(JwtRegisteredClaimNames.Sub, model.password)
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              claims: claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
