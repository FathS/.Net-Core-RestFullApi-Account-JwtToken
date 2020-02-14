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
        public IActionResult CityAdd([FromBody]City city)
        {
            if (string.IsNullOrEmpty(city.Name))
            {
                return BadRequest("İsim Boş Bırakılamaz");
            }

            //var c = new City
            //{
            //    Name = city.name,
            //};

            _db.Entry(city).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();
            return Ok("İl Ekleme Başarılı");
        }


        public IActionResult CityList()
        {

            var cities = _db.Set<City>().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            }).OrderBy(x => x.Text);

            return Ok(cities);
        }

        [HttpPost]
        public IActionResult DistrictAdd([FromBody] District district)
        {
            if (string.IsNullOrEmpty(district.Name))
            {
                return BadRequest("İlçe İsmi Boş Bırakılamaz.");
            }
            if (district.CityId == null)
            {
                return BadRequest("Hata!");
            }

            //District dist = new District
            //{
            //    Name = model.name,
            //    CityId = model.cityId
            //};
            _db.Entry(district).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();

            return Ok("İlçe Ekleme Başarılı");
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