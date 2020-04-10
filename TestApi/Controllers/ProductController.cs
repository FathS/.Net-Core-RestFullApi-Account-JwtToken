using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTO;
using TestApi.Models.Data;
using TestApi.Models.Data.Entities;

namespace TestApi.Controllers
{
    public class ProductController : Controller
    {
        private readonly PersonelContext _db;
        private readonly IHostingEnvironment _environment;

        public ProductController(PersonelContext db, IHostingEnvironment env)
        {
            _db = db;
            _environment = env;
        }


        public IActionResult ProductList(Guid id)
        {
            var products = _db.Set<Products>().Where(x => x.CategoryId == id).Select(x => new ProductModel
            {
                name = x.ProductName,
                price = x.Price,
                amount = x.Amount,
                size = x.Size,
                title = x.Title,
                //image = x.Image
            }).ToList();

            if (products.Count == 0)
            {
                return BadRequest("Ürün Bulunamadı");
            }

            return Json(products);
        }


        [HttpPost]
        public IActionResult ProductCreate([FromBody] ProductModel model)
        {

            if (model.file == null)
            {
                return BadRequest("Dosya Bulunamadı");
            }

            Products product = new Products
            {
                Id = new Guid(),
                ProductName = model.name,
                Size = model.size,
                Amount = model.amount,
                Price = model.price,
                Title = model.title,
                CategoryId = model.categoryid,
            };
            _db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();

            return Json("Kayıt Başarılı");
        }


        [HttpPost]
        public IActionResult imageadd(IFormFile image)
        {
            string fileName = Guid.NewGuid().ToString();
            if (image != null)
            {
                var Upload = Path.Combine(_environment.WebRootPath, "C:/Users/is97788/Desktop/BasicVueJs/BasicVueJs/src/assets/images", fileName);
                image.CopyTo(new FileStream(Upload, FileMode.Create));

                return Json("Ok");
            }
            return Ok();
        }
    }
}