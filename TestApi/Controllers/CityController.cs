using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestApi.DTO;
using TestApi.Models.Data;
using TestApi.Models.Data.Entities;

namespace TestApi.Controllers
{
    public class CityController : Controller
    {
        private readonly PersonelContext _db;

        public CityController(PersonelContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult CityAdd([FromBody]CityModel city)
        {
            if (string.IsNullOrEmpty(city.name))
            {
                return BadRequest("İsim Boş Bırakılamaz");
            }

            var c = new City
            {
                Id = city.id,
                Name = city.name,
            };

            _db.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return Ok("Kayıt Başarılı");
        }
        public IActionResult CityList()
        {

            var cities = _db.Set<City>().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,

            });

            return Ok(cities);
        }
        public IActionResult DistrictList(int id)
        {
            var district = _db.Set<District>().Select(x => new DistrictModel
            {
                id = x.Id,
                name = x.Name,
                cityId = x.CityId
            }).Where(x => x.cityId == id).OrderBy(x => x.name).ToList();

            return Json(district);
        }
    }
}