using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace codeweb.Controllers
{
    public class AdminPanelController : Controller
    {
        private Model1 database = new Model1();
        // GET: AdminPanel
        public ActionResult Index(int? month)
        {
            // Get the total sales amount
            decimal totalSales = database.OrderProes.Select(o => (decimal?)o.TotalMoney).DefaultIfEmpty(0M).Sum().Value;

            // Get the total number of orders
            int totalOrders = database.OrderProes.Count();

            ViewBag.TotalSales = totalSales;
            ViewBag.TotalOrders = totalOrders;

            return View();
        }
    }
}