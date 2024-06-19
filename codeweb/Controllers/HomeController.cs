using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace codeweb.Controllers
{
    public class HomeController : Controller
    {

        private DoAnEntities1 db = new DoAnEntities1();

        public ActionResult Index()
        {
            var clothes = db.Clothes.Take(10);
            return View(clothes);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}