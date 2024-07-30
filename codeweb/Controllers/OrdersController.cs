using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace codeweb.Controllers
{
        public class OrderController : Controller
        {
            private Model1 db = new Model1();

            // GET: Order
            public ActionResult Index()
            {
                var orders = db.OrderProes.ToList();
                return View(orders);
            }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }


        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(OrderPro orderPro)
        {
           
                if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (ModelState.IsValid)
                {
                    db.Entry(orderPro).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("OrdersManagement","AdminPanel");
                }
                return View(orderPro);
        }


        public ActionResult Delete(int? id)
        {
            if (Session["IsAdmin"] == null || (bool)Session["IsAdmin"] == false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["IsAdmin"] == null || (bool)Session["IsAdmin"] == false)
            {
                return RedirectToAction("Index", "Home");
            }

            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro != null)
            {
                // Find and delete related order details
                var relatedOrderDetails = db.OrderDetails.Where(od => od.IDOrder == id);
                db.OrderDetails.RemoveRange(relatedOrderDetails);

                // Now it's safe to remove the order
                db.OrderProes.Remove(orderPro);
                db.SaveChanges();
                return RedirectToAction("OrdersManagement", "AdminPanel");
            }
            return View(orderPro);
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