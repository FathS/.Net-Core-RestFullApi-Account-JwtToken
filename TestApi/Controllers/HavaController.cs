using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using TestApi.DTO;

namespace TestApi.Controllers
{
    public class HavaController : Controller
    {
        public IActionResult Durum(string cityName, int pageSize, int CurrentPage)

        {
            if (cityName != null)
            {
                var HavaList = new List<havaModel>();
                try
                {
                    XDocument xDoc = XDocument.Load("https://www.mgm.gov.tr/FTPDATA/analiz/sonSOA.xml");
                    HavaList = xDoc.Descendants("sehirler").Where(x => (string)x.Element("Merkez") == cityName).Select(x => new havaModel
                    {
                        durum = (string)x.Element("Durum"),
                        il = (string)x.Element("ili"),
                        mak = (string)x.Element("Mak"),
                    }).ToList();
                    return Ok(HavaList);
                }
                catch (Exception)
                {
                    return BadRequest("Veri Çekilemedi");
                }
            }
            else
            {
                var HavaList = new List<havaModel>();
                try
                {
                    XDocument xDoc = XDocument.Load("https://www.mgm.gov.tr/FTPDATA/analiz/sonSOA.xml");
                    HavaList = xDoc.Descendants("sehirler").Skip((CurrentPage - 1) * pageSize).Take(pageSize).Select(x => new havaModel
                    {
                        //Bolge = (string)o.Element("Bolge"),
                        //Merkez = (string)o.Element("Merkez"),
                        durum = (string)x.Element("Durum"),
                        il = (string)x.Element("ili"),
                        mak = (string)x.Element("Mak"),
                        bolge = (string)x.Element("Bolge"),
                    }).ToList();
                    return Ok(HavaList);
                }
                catch (Exception)
                {
                    return BadRequest("Veri Çekilemedi");
                }
            }

        }
    }
}