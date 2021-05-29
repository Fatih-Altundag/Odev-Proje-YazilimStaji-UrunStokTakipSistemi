using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokTakipProjesi.Models;

namespace UrunStokTakipProjesi.Controllers
{
    [Authorize(Roles = "A")]

    public class KategoriController : Controller
    {
        // GET: Kategori
        Urun_Takip_SistemiEntities1 db = new Urun_Takip_SistemiEntities1();
        // [Authorize(Roles = "A")]       
        public ActionResult Index()
        {
            return View(db.Kategori.Where(x => x.Durum == true).ToList());  
        }
       // [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        // [Authorize(Roles = "A")]
        public ActionResult Ekle(Kategori data)
        {
            db.Kategori.Add(data);
            data.Durum = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // [Authorize(Roles = "A")]

        public ActionResult Sil(int id)
        {
            var kategori = db.Kategori.Where(x => x.Id == id).FirstOrDefault();
            db.Kategori.Remove(kategori);
            kategori.Durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Guncelle(Kategori data)
        {
            var guncelle = db.Kategori.Where(x => x.Id == data.Id).FirstOrDefault();
            guncelle.Aciklama = data.Aciklama;
            guncelle.Ad = data.Ad;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}