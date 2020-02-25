using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestApi.DTO;
using TestApi.Helpers;
using TestApi.Models.Data;
using TestApi.Models.Data.Entities;

namespace TestApi.Controllers
{
    public class AccountController : Controller
    {
        private readonly PersonelContext _db;
        private IConfiguration _config;
        public AccountController(PersonelContext db, Users user, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        [HttpPost]
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
            string Defaultrole;
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest("İsim Boş Bırakılamaz");
            }
            if (string.IsNullOrEmpty(model.Surname))
            {
                return BadRequest("Soyİsim Boş Bırakılamaz");
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Email Boş Bırakılamaz");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Parola Boş Bırakılamaz");
            }
            if (string.IsNullOrEmpty(model.ConfirPassword))
            {
                return BadRequest("Parola Tekrar Boş Bırakılamaz");
            }
            if (model.Password != model.ConfirPassword)
            {
                return BadRequest("Parola Eşleşmedi!");
            }
            if (!model.isActive)
            {
                return BadRequest("Sözleşmeyi Onaylamanız Gerekmektedir.");
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

            if (model.role == null)
            {
                Defaultrole = "User";
            }
            else
            {
                Defaultrole = model.role;
            }

            if (ModelState.IsValid)
            {
                var user = new Account
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Surname = model.Surname.ToUpperInvariant(),
                    Age = model.Age,
                    Email = model.Email,
                    isActive = model.isActive,
                    Role = Defaultrole
                };


                _db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                CreatePassword(user.Id, model.Password, model.ConfirPassword);
                _db.SaveChanges();
                return Json(message);

                //if (model.Password == model.ConfirPassword)
                //{
                //    model.isActive = true;
                //    _db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                //    _db.SaveChanges();
                //    return Json(message);
                //}
            }
            return BadRequest();
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
                role = account.Role
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
              expires: DateTime.Now.AddSeconds(15),
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
            var account = _db.Set<Account>().Find(model.id);

            if (account == null)
            {
                return BadRequest("Hesap Bulunamadı");
            }

            account.isActive = model.isActive;
            _db.SaveChanges();
            return Ok("Hesabınız Dondurulmuştur.");


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

        public IActionResult DovizCevir(Guid id, decimal TL, string birim)
        {
            if (TL < 10)
            {
                return BadRequest("10 TL ve üzeri girilen miktarlarda dolar alınabilir.");
            }

            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");
            if (birim == "Dolar")
            {
                decimal Dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
                var doviz = TL / Dolar;
                var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);

                if (account == null)
                {
                    return BadRequest("Kullanıcı Bulunamadı");
                }
                if (account.TL < TL)
                {
                    return BadRequest("Hesabınızda Yeterli Tutar Bulunmamaktadır. Hesabınızdaki Tutar:" + account.TL);
                }

                var kalanTl = account.TL - TL;
                var dovizMiktar = account.USD + doviz;

                //account.USD += doviz;
                account.USD = dovizMiktar;
                account.TL = kalanTl;
                _db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var balance = new Balance
                {
                    AccountId = id,
                    BuyUSD = doviz,
                    SellTL = TL,
                    OperationTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString(),
                    DolarKur = Dolar
                };
                _db.Entry(balance).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _db.SaveChanges();
                var message = "Hesap Bilgisi:" + " " + TL + "TL 'ye" + " " + doviz + " USD alındı. " + "TL Bakiyeniz: " + account.TL + " UDS Bakiyeniz : " + account.USD;

                return Ok(message);
            }
            if (birim == "Euro")
            {
                decimal Euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
                var doviz = TL / Euro;
                var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);

                if (account == null)
                {
                    return BadRequest("Kullanıcı Bulunamadı");
                }
                if (account.TL < TL)
                {
                    return BadRequest("Hesabınızda Yeterli Tutar Bulunmamaktadır. Hesabınızdaki Tutar:" + account.TL);
                }

                var kalanTl = account.TL - TL;
                var dovizMiktar = account.EURO + doviz;

                //account.USD += doviz;
                account.EURO = dovizMiktar;
                account.TL = kalanTl;
                _db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var balance = new Balance
                {
                    AccountId = id,
                    BuyUSD = doviz,
                    SellTL = TL,
                    OperationTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString(),
                    EuroKur = Euro
                };
                _db.Entry(balance).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _db.SaveChanges();
                var message = "Hesap Bilgisi:" + " " + TL + "TL 'ye" + " " + doviz + " EURO alındı. " + "TL Bakiyeniz: " + account.TL + " EURO Bakiyeniz : " + account.EURO;

                return Ok(message);
            }
            return Ok();
        }

        public IActionResult DovizSat(Guid id, decimal döviz, string birim)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("Kullanıcı Bulunamadı");
            }
            if (string.IsNullOrEmpty(birim))
            {
                return BadRequest("Bozdurmak istediğiniz Dövizi Seçmeniz Gerekmektedir.");
            }
            if (döviz < 2 && birim == "Dolar")
            {
                return BadRequest("En az 2 Dolar Bozdurabilirsiniz");
            }
            if (döviz < 2 && birim == "Euro")
            {
                return BadRequest("En az 2 Euro Bozdurabilirsiniz");
            }

            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");
            if (birim == "Dolar")
            {
                decimal Dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));

                var bozdurulan = döviz * Dolar;

                var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);

                if (account == null)
                {
                    return BadRequest("Hesap Bulunamadı");
                }
                if (account.USD < döviz)
                {
                    return BadRequest("Hesabınızda Girdiğiniz değer kadar dolar bulunmamaktadır. Lütfen Bakiyenizi Kontrol edip tekrar deneyiniz.");
                }
                var kalanDoviz = account.USD - döviz;
                var kalanTl = account.TL + bozdurulan;

                account.USD = kalanDoviz;
                account.TL = kalanTl;

                _db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                var balance = new Balance
                {
                    AccountId = id,
                    DolarKur = Dolar,
                    OperationTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString(),
                    BuyTL = bozdurulan,
                    SellUSD = döviz,
                };
                _db.Entry(balance).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _db.SaveChanges();
                return Ok(döviz + " " + birim + " " + bozdurulan + " TL Parasına Çevirildi.");
            }
            if (birim == "Euro")
            {
                decimal Euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
                var bozdurulan = döviz * Euro;

                var account = _db.Set<Account>().FirstOrDefault(x => x.Id == id);

                if (account == null)
                {
                    return BadRequest("Hesap Bulunamadı");
                }
                if (account.EURO < döviz)
                {
                    return BadRequest("Hesabınızda Girdiğiniz değer kadar Euro bulunmamaktadır. Lütfen Bakiyenizi Kontrol edip tekrar deneyiniz.");
                }
                var kalanDoviz = account.EURO - döviz;
                var kalanTl = account.TL + bozdurulan;

                account.EURO = kalanDoviz;
                account.TL = kalanTl;

                _db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                var balance = new Balance
                {
                    AccountId = id,
                    EuroKur = Euro,
                    OperationTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString(),
                    BuyTL = bozdurulan,
                    SellEURO = döviz

                };
                _db.Entry(balance).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _db.SaveChanges();
                return Ok(döviz + " " + birim + " " + bozdurulan + " TL Parasına Çevirildi.");
            }

            return Ok();
        }

        public IActionResult getBakiye(Guid id)
        {
            var userbakiye = _db.Set<Account>().FirstOrDefault(x => x.Id == id);

            var model = new bakiyeModel
            {
                tl = userbakiye.TL,
                usd = userbakiye.USD,
                euro = userbakiye.EURO
            };

            return Ok(model);
        }
        public IActionResult getHesapHareket(Guid id)
        {
            var hareketList = _db.Set<Balance>().Where(x => x.AccountId == id).Select(x => new balanceListModel
            {
                user = x.Account.Name + " " + x.Account.Surname,
                buyUsd = x.BuyUSD,
                sellTl = x.SellTL,
                date = x.OperationTime,
                dolarKur = x.DolarKur,
                euroKur = x.EuroKur,
                buyTl = x.BuyTL,
                selleuro = x.SellEURO,
                sellUsd = x.SellUSD
            }).ToList();

            if (hareketList.Count == 0)
            {
                return BadRequest("Hesap Hareketi Bulunamadı");
            }

            return Ok(hareketList);
        }



    }
}