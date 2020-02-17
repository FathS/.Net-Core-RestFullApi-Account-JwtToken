using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
                IsActive = x.IsActive,
                district = x.District.Name
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
                return BadRequest("İsim Boş Bırakılamaz!");
            }
            if (string.IsNullOrEmpty(user.surname))
            {
                return BadRequest("Soyisim Boş Bırakılamaz!");
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
                DistrictId = user.districtId
            };

            _db.Entry(users).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return Ok("Kayıt Başarılı!");
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

        public IActionResult CreatePassword(Guid UserId, string Password, string ConfirmPassword)
        {

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

        [HttpPost]
        public IActionResult Register([FromBody] registerModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest("İsim Boş Bırakılamaz");
            }

            var message = "Kayıt İşlemi Başarılı";
            var test = _db.Set<Account>().FirstOrDefault(x => x.Email == model.Email);

            if (test != null)
            {
                if (test.Email == model.Email)
                {
                    return BadRequest("Aynı Mail Adresi Sistemde Bulunmaktadır. Farklı bir mail adresi ile kayıt olmayı deneyiniz");
                }
            }


            if (ModelState.IsValid)
            {
                var user = new Account
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Surname = model.Surname,
                    Age = model.Age,
                    CreateTime = DateTime.Now,
                    Email = model.Email,
                    isActive = true,

                };

                _db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                CreatePassword(user.Id, model.Password, model.ConfirPassword);
                _db.SaveChanges();
                return Json(message);
            }
            return BadRequest("Kayıt Yapılamadı!");
        }

        [HttpPost]
        public IActionResult Login([FromBody] loginModel model)
        {
            string message = "Email Adresi hatalı veya kayıtlı değil";
            if (string.IsNullOrEmpty(model.email))
            {
                return BadRequest("Kullanıcı Adı Boş Bırakılamaz");
            }
            if (string.IsNullOrEmpty(model.password))
            {
                return BadRequest("Parola Boş Bırakılamaz");
            }

            var account = _db.Set<Account>().FirstOrDefault(x => x.Email == model.email);

            if (account == null)
            {
                return BadRequest(message);
            }
            if ((bool)!account.isActive)
            {
                return BadRequest("Hesap Aktif Değil. Aktif Etmek için Tıklayınız");
            }
            var password = _db.Set<UserPassword>().FirstOrDefault(x => x.UserId == account.Id && x.Password == model.password);
            if (password == null)
            {
                return BadRequest("Parola Hatalı");
            }

            if (!password.ActivePassword)
            {
                return BadRequest("Eski Kullandığınız parolayı girdiniz. Lütfen Güncel parolanızı giriniz.");
            }

            var tokenString = GenerateJSONWebToken(model);

            var loginApi = new LoginApiModel
            {
                id = account.Id,
                name = account.Name,
                surname = account.Surname,
                token = tokenString,
            };
            return Json(loginApi);


        }
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
              expires: DateTime.Now.AddMinutes(185),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //private void HashingPassword(string password)
        //{
        //    var rng = RandomNumberGenerator.Create();
        //    var saltBytes = new byte[16];
        //    rng.GetBytes(saltBytes);
        //    var saltText = Convert.ToBase64String(saltBytes);

        //    var sha = SHA256.Create();
        //    var saltedPassord = password + saltText;
        //    var saltedhashedPassword = Convert.ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassord)));

        //    var User = new UserPassword
        //    {
        //        Salt = saltText,
        //        SaltedHashedPassword = saltedhashedPassword,
        //    };
        //}

        [HttpPost]
        public IActionResult ChangePassword([FromBody] changePassModel model)
        {
            if (model.id == null)
            {
                return BadRequest("Geçerli Kullanıcı Bulunamadı!");
            }
            if (string.IsNullOrEmpty(model.oldPassword))
            {
                return BadRequest("Lütfen Eski Parolanızı Giriniz.");
            }
            if (string.IsNullOrEmpty(model.password))
            {
                return BadRequest("Lütfen Parola Belirleyiniz.");
            }
            if (model.password.Length < 8)
            {
                return BadRequest("Parolanız En Az 8 Haneli Olmak Zorundadır.");
            }
            if (string.IsNullOrEmpty(model.confirPassword))
            {
                return BadRequest("Lütfen Parolanızı Tekrar Giriniz.");
            }
            if (model.password != model.confirPassword)
            {
                return BadRequest("Parolanız Eşleşmedi!");
            }
            var ps = _db.Set<UserPassword>().FirstOrDefault(x => x.UserId == model.id && x.Password == model.oldPassword && x.ActivePassword);

            if (ps == null)
            {
                return BadRequest("Parola Hatalı");
            }

            if (ps.Password == model.oldPassword)
            {
                ps.ActivePassword = false;
                CreatePassword(model.id, model.password, model.confirPassword);
                return Ok();
            }
            else
            {
                return BadRequest("Eski Parolanız Hatalı");
            }


        }
        [HttpPost]
        public IActionResult DisabledAccount([FromBody] DisabledModel model)
        {
            var account = _db.Set<Account>().FirstOrDefault(x => x.Id == model.id);

            if (account == null)
            {
                return BadRequest("Hesap Bulunamadı");
            }
            else
            {
                account.isActive = model.isActive;
                _db.SaveChanges();
                return Ok("Hesap Devre Dışı Bırakıldı");
            }

        }
        [HttpPost]
        public IActionResult ActiveAccount([FromBody] ActiveModel model)
        {
            var account = _db.Set<Account>().FirstOrDefault(x => x.Email == model.email);
            if (account == null)
            {
                return BadRequest("Sistemde tanımlı hesap bulunamadı!");
            }
            var password = _db.Set<UserPassword>().FirstOrDefault(x => x.UserId == account.Id && x.Password == model.password);
            if (password == null)
            {
                return BadRequest("Şifre Hatalı veya Aktif Değil!");
            }
            else
            {
                account.isActive = model.active;
                _db.SaveChanges();
                return Ok("Hesap Aktif Edildi.");
            }

        }
    }
}
