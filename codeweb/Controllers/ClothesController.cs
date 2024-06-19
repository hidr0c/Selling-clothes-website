using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using codeweb.Models;

namespace codeweb.Controllers
{
    public class ClothesController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();

        public PartialViewResult PartialProduct(Cloth product)
        {
            return PartialView(product);
        }

        // GET: Clothes
        public ActionResult Index()
        {
            var clothes = db.Clothes.Include(c => c.Brand).Include(c => c.Category);
            return View(clothes.ToList());
        }

        // GET: Clothes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cloth cloth = db.Clothes.Find(id);
            if (cloth == null)
            {
                return HttpNotFound();
            }
            return View(cloth);
        }

        // GET: Clothes/Create
        public ActionResult Create()
        {
            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "NameBrand");
            ViewBag.IDCategory = new SelectList(db.Categories, "IDCategory", "NameCategory");
            return View();
        }

        // POST: Clothes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDClothes,NameClothes,BuyPrice,SoldPrice,Discount,FinalSoldPrice,ClothesImage,Tax,IDBrand,IDCategory")] Cloth cloth)
        {
            if (ModelState.IsValid)
            {
                db.Clothes.Add(cloth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "NameBrand", cloth.IDBrand);
            ViewBag.IDCategory = new SelectList(db.Categories, "IDCategory", "NameCategory", cloth.IDCategory);
            return View(cloth);
        }

        // GET: Clothes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cloth cloth = db.Clothes.Find(id);
            if (cloth == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "NameBrand", cloth.IDBrand);
            ViewBag.IDCategory = new SelectList(db.Categories, "IDCategory", "NameCategory", cloth.IDCategory);
            return View(cloth);
        }

        // POST: Clothes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDClothes,NameClothes,BuyPrice,SoldPrice,Discount,FinalSoldPrice,ClothesImage,Tax,IDBrand,IDCategory")] Cloth cloth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cloth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "NameBrand", cloth.IDBrand);
            ViewBag.IDCategory = new SelectList(db.Categories, "IDCategory", "NameCategory", cloth.IDCategory);
            return View(cloth);
        }

        // GET: Clothes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cloth cloth = db.Clothes.Find(id);
            if (cloth == null)
            {
                return HttpNotFound();
            }
            return View(cloth);
        }

        // POST: Clothes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Cloth cloth = db.Clothes.Find(id);
            db.Clothes.Remove(cloth);
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
