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

    public class CategoryController : Controller
    {
        private readonly PersonelContext _db;
        public CategoryController(PersonelContext db)
        {
            _db = db;
        }


        public IActionResult CategoryList()
        {
            var categories = _db.Set<Category>().Select(x => new CategoryListModel
            {
                name = x.CategoryName,
                categoryid = x.Id

            }).ToList();

            return Json(categories);
        }
        [HttpPost]
        public IActionResult CategoryCreate([FromBody] NameModel model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                return BadRequest("İsim Boş Bırakılamaz.");
            }
            Category category = new Category
            {
                Id = new Guid(),
                CategoryName = model.name
            };
            _db.Add(category);
            _db.SaveChanges();

            return Ok("Kayıt Başarılı");
        }
    }
}