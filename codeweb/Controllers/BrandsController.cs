using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace codeweb.Controllers
{
    public class BrandsController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Index()
        {
            var Brand = db.Brands.ToList();
            if (ControllerContext.IsChildAction)
            {
                return PartialView(Brand.ToList());
            }
            return RedirectToAction("Details", new { id = "1" });
        }

        public ActionResult Details(string id, int page = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            IEnumerable<Product> productList = brand.Products.ToList();

            int maxPage = Math.Max(1, productList.Count() / 10);
            if (page > maxPage)
            {
                page = maxPage;
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.CurrentPage = page;

            var tuple = new Tuple<Brand, IEnumerable<Product>>(brand, productList.Skip((page - 1) * 10).Take(10));
            return View(tuple);
        }

        public ActionResult Create()
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBrand,Id,BrandName")] Brand brand)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        public ActionResult Edit(string id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBrand,Id,BrandName")] Brand brand)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        public ActionResult Delete(string id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}