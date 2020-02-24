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
            var inv = _db.Set<Inventory>().FirstOrDefault(x => x.SeriNo == model.seriNo);

            if (inv != null)
            {
                if (inv.SeriNo == model.seriNo)
                {
                    return BadRequest("Bu Seri Numaraya Ait Ürün Bulunmaktadır. Lütfen Seri Numarayı Kontrol Edip Tekrar Deneyiniz.");
                }
            }

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

        public IActionResult GetInventory(Guid id)
        {
            var invList = _db.Set<Inventory>().Where(x => x.AccountId == id).Select(x => new InventoryListModel
            {
                name = x.Name,
                marka = x.Marka,
                model = x.Model,
                seriNo = x.SeriNo,
                feature = x.Feature,
                createTime = x.CreateTime,
                accountMail = x.Account.Name + " " + x.Account.Surname
            }).ToList();

            
            if (invList.Count == 0)
            {
                return BadRequest("Kullanıcıya ait envanter bulunamadı!");
            }

            return Ok(invList);
        }
        [HttpGet]
        public IActionResult AddTransfer(Guid Id, Guid UserId)
        {
            if (Guid.Empty == Id || Guid.Empty == UserId)
            {
                return BadRequest("İşlem Gerçekleştirilemedi.");
            }

            AddInv(Id, UserId);

            return Ok("Envanter Atama Gerçekleştirildi.");
        }

        private string AddInv(Guid Inventoryid, Guid UserId)
        {
            var inventory = _db.Set<Inventory>().FirstOrDefault(x => x.Id == Inventoryid);

            inventory.AccountId = UserId;
            inventory.Status = true;

            _db.Entry(inventory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return "Başarılı";
        }
    }
}
