using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokTakipProjesi.Models;

namespace UrunStokTakipProjesi.Controllers
{
    public class TumSatislarController : Controller
    {
        // GET: TumSatislar
        Urun_Takip_SistemiEntities1 db = new Urun_Takip_SistemiEntities1();
        [Authorize(Roles = "A")]
        public ActionResult Index(int sayfa = 1)
        {
            return View(db.Satislar.ToList().ToPagedList(sayfa, 5));
        }
    }
}