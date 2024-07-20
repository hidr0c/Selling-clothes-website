using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

            // GET: Order/Edit/5
            public ActionResult Edit(int id)
            {
                var order = db.OrderProes.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }

            // POST: Order/Edit/5
            [HttpPost]
            public ActionResult Edit(OrderPro order)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(order);
            }

            // GET: Order/Delete/5
            public ActionResult Delete(int id)
            {
                var order = db.OrderProes.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }

            // POST: Order/Delete/5
            [HttpPost, ActionName("Delete")]
            public ActionResult DeleteConfirmed(int id)
            {
                var order = db.OrderProes.Find(id);
                db.OrderProes.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }