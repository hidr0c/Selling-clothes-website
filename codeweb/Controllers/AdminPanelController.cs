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
            // Get the total number of orders
            int totalOrders = database.OrderProes.Count();

            // Count orders by status
            int waitingForConfirmationOrders = database.OrderProes.Count(o => o.Status == "Waiting for confirmation");
            int onDeliveryOrders = database.OrderProes.Count(o => o.Status == "On delivery");
            int completedOrders = database.OrderProes.Count(o => o.Status == "Completed");

       /*     // Get the total sales amount from completed orders
            decimal totalSales = database.OrderProes
                .Where(o => o.Status == "Completed")
                .Select(o => (decimal?)o.TotalMoney)
                .DefaultIfEmpty(0M)
                .Sum()
                .GetValueOrDefault();*/

      /*      // Calculate total revenue after order status is "Completed"
            decimal totalRevenueAfterCompleted = database.OrderProes
                .Where(o => o.Status == "Completed")
                .Select(o => (decimal?)o.TotalMoney)
                .DefaultIfEmpty(0M)
                .Sum()
                .GetValueOrDefault();*/

            decimal totalSales = database.OrderProes.Where(o => o.Status == "Completed").Select(o => (decimal?)o.TotalMoney).DefaultIfEmpty(0M).Sum().Value;


            ViewBag.TotalSales = totalSales;
            ViewBag.TotalOrders = totalOrders;

            ViewBag.WaitingForConfirmationOrders = waitingForConfirmationOrders;
            ViewBag.OnDeliveryOrders = onDeliveryOrders;
            ViewBag.CompletedOrders = completedOrders;

            return View();
        }
        public ActionResult ProductsManagement()
        {
            return View();
        }
        public ActionResult BrandsManagement()
        {
            return View();
           
        }
        public ActionResult CustomersManagement()
        {
            return View();
        }
        public ActionResult OrdersManagement()
        {
            return View();

        }
    }
}