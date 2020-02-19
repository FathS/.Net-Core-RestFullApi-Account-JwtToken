using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestApi.DTO;

namespace TestApi.Controllers
{
    public class DovizController : Controller
    {
        public IActionResult Index()
        {
            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");

            decimal Dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
            decimal Euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));

            var doviz = new dovizModel
            {
                dolar = Dolar.ToString(),
                euro = Euro.ToString(),
                date = DateTime.Now.ToLocalTime(),
            };

            var DovizList = new List<dovizModel>();

            DovizList.Add(doviz);

            return Ok(DovizList);
        }
        
        
    }
}