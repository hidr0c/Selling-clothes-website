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

        /* if (Session["IsAdmin"] == null || Session["IsAdmin"] is false)
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

         // Cập nhật orderPro với dữ liệu mới từ form
         orderPro.Status = collection["orderStatus"];
         orderPro.AddressDelivery = collection["AddressDelivery"];
         orderPro.PhoneNumber = int.Parse(collection["PhoneNumber"]);
         orderPro.NameCus = collection["NameCus"];

         // Lưu thay đổi vào cơ sở dữ liệu
         db.Entry(orderPro).State = EntityState.Modified;
         db.SaveChanges();

         return RedirectToAction("Index");*/



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

            // Xóa tất cả các chi tiết đơn hàng liên quan
            foreach (var detail in order.OrderDetails.ToList())
            {
                db.OrderDetails.Remove(detail);
            }

            db.OrderProes.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            OrderPro order = db.OrderProes.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


    }

    }