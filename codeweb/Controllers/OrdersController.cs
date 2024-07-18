using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace codeweb.Controllers
{
    public class OrdersController : Controller
    {
        private Model1 db = new Model1();

        public PartialViewResult PartialProduct(OrderPro orderPro)
        {
            return PartialView(orderPro);
        }

        public ActionResult Index()
        {
            var orderPro = db.OrderPro.ToList();
            if (ControllerContext.IsChildAction)
            {
                return PartialView(orderPro.ToList());
            }
            return RedirectToAction("Details", new { id = "1" });
        }
    }
}