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
    public class InventoryController : Controller
    {
        private readonly PersonelContext _db;
        public InventoryController(PersonelContext db)
        {
            _db = db;
        }
        public IActionResult GetList()
        {
            var invList = _db.Set<Inventory>().Select(x => new InventoryListModel
            {
                name = x.Name,
                model = x.Model,
                marka = x.Marka,
                feature = x.Feature,
                seriNo = x.SeriNo,
                status = x.Status,
                createTime = x.CreateTime,
                id = x.Id,
                accountMail = x.Account.Email
            }).ToList();
            return Ok(invList);
        }
        [HttpPost]
        public IActionResult AddInventory([FromBody] InventoryListModel model)
        {
            var inventory = new Inventory
            {
                Id = new Guid(),
                Marka = model.marka,
                Feature = model.feature,
                Name = model.name,
                Model = model.model,
                SeriNo = model.seriNo,
                Status = false,
            };
            _db.Entry(inventory).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();

            return Ok("Envanter Sisteme Eklendi");
        }
    }
}