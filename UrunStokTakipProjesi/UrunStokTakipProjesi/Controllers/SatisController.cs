﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokTakipProjesi.Models;
using PagedList;
using PagedList.Mvc;

namespace UrunStokTakipProjesi.Controllers
{
    public class SatisController : Controller
    {
        // GET: Satis
        Urun_Takip_SistemiEntities1 db = new Urun_Takip_SistemiEntities1();
        public ActionResult Index(int sayfa = 1)
        {

            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.Email == kullaniciadi);
                var model = db.Satislar.Where(x => x.KullaniciId == kullanici.Id).ToList().ToPagedList(sayfa, 5);
                return View(model);
            }
            return HttpNotFound();

        }
        public ActionResult SatinAl(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.Id == id);
            return View(model);
        }
        [HttpPost]

        public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.Id == id);

                    var satis = new Satislar
                    {
                        KullaniciId = model.KullaniciId,
                        UrunId = model.UrunId,
                        Adet = model.Adet,
                        Resim = model.Resim,
                        Fiyat = model.Fiyat,
                        Tarih = model.Tarih,
                    };
                    db.Sepet.Remove(model);
                    db.Satislar.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satın Alma İşlemi Başarılı Bir Şekilde Gerçekleşmiştir";
                }
            }
            catch (Exception)
            {
                ViewBag.islem = "Satın Alma İşlemi Başarısız";
            }
            return View("islem");
        }
        public ActionResult HepsiniSatinAl(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.Email == kullaniciadi);
                var model = db.Sepet.Where(x => x.KullaniciId == kullanici.Id).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.KullaniciId == kullanici.Id);

                if (model != null)
                {


                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmamaktadır";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullaniciId == kid.KullaniciId).Sum(x => x.Urun.Fiyat * x.Adet);
                        ViewBag.Tutar = "Toplam Tutar = " + Tutar + "TL";
                    }
                    return View(model);
                }
                return View();
            }
            return HttpNotFound();

        }
        [HttpPost]

        public ActionResult HepsiniSatinAl2()
        {
            var username = User.Identity.Name;
            var kullanici = db.Kullanici.FirstOrDefault(x => x.Email == username);
            var model = db.Sepet.Where(x => x.KullaniciId == kullanici.Id).ToList();
            int satir = 0;
            foreach (var item in model)
            {
                var satis = new Satislar
                {
                    KullaniciId = model[satir].KullaniciId,
                    UrunId = model[satir].UrunId,
                    Adet = model[satir].Adet,
                    Fiyat = model[satir].Fiyat,
                    Resim = model[satir].Urun.Resim,
                    Tarih = DateTime.Now
                };
                db.Satislar.Add(satis);
                db.SaveChanges();
                satir++;
            }
            db.Sepet.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }

    }
}
 
    

