using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickType;
using TestApi.DTO;

namespace TestApi.Controllers
{
    public class DovizController : Controller
    {
        public IActionResult Index()
        {
            //XmlDocument xmlVerisi = new XmlDocument();
            //xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");

            //decimal Dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
            //decimal Euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
            string adres = "https://finans.truncgil.com/today.json";
            WebRequest istek = HttpWebRequest.Create(adres); // istek yolladık
            WebResponse cevap = istek.GetResponse(); // cevabı aldık
            StreamReader donenBilgiler = new StreamReader(cevap.GetResponseStream()); // cevabı okuduk
            string bilgilerial = donenBilgiler.ReadToEnd(); // okuduğumuz cevabı stringe atadık
            dövizNewModel veriler = JsonConvert.DeserializeObject<dövizNewModel>(bilgilerial);

            var doviz = new dovizModel
            {
                dolar = veriler.AbdDolari.Satış,
                euro = veriler.Euro.Satış,
                dolarAlis = veriler.AbdDolari.Alış,
                euroAlis = veriler.Euro.Alış,
                altin = Decimal.Parse(veriler.GramAltın.Satış.Replace(".", ",")),
                altinAlis = veriler.GramAltın.Alış

            };

            var DovizList = new List<dovizModel>();

            DovizList.Add(doviz);

            return Ok(doviz);
        }

        public IActionResult getDövizList()
        {
            //string path = "https://finans.truncgil.com/today.json";
            //StreamReader stream_read = new StreamReader(path);
            //string js_data = stream_read.ReadToEnd();// dosyayı okuruz.
            //List<dövizNewModel> veriler = JsonConvert.DeserializeObject<List<dövizNewModel>>(js_data);

            string adres = "https://finans.truncgil.com/today.json";
            WebRequest istek = HttpWebRequest.Create(adres); // istek yolladık
            WebResponse cevap = istek.GetResponse(); // cevabı aldık
            StreamReader donenBilgiler = new StreamReader(cevap.GetResponseStream()); // cevabı okuduk
            string bilgilerial = donenBilgiler.ReadToEnd(); // okuduğumuz cevabı stringe atadık
            dövizNewModel veriler = JsonConvert.DeserializeObject<dövizNewModel>(bilgilerial);


            return Json(veriler);
        }
        public IActionResult ChartList()
        {
            string adres = "https://finans.truncgil.com/today.json";
            WebRequest istek = HttpWebRequest.Create(adres); // istek yolladık
            WebResponse cevap = istek.GetResponse(); // cevabı aldık
            StreamReader donenBilgiler = new StreamReader(cevap.GetResponseStream()); // cevabı okuduk
            string bilgilerial = donenBilgiler.ReadToEnd(); // okuduğumuz cevabı stringe atadık
            dövizNewModel veriler = JsonConvert.DeserializeObject<dövizNewModel>(bilgilerial);

            var doviz = new dovizModel
            {
                dolar = veriler.AbdDolari.Satış,
                //euro = veriler.Euro.Satış,
                //dolarAlis = veriler.AbdDolari.Alış,
                //euroAlis = veriler.Euro.Alış,
                //altin = Decimal.Parse(veriler.GramAltın.Satış.Replace(".", ",")),
                //altinAlis = veriler.GramAltın.Alış

            };

            var DovizList = new List<dovizModel>();

            DovizList.Add(doviz);

            return Ok(DovizList);
        }
    }
}