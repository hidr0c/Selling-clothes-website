using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Routing;

namespace KetNoiDatabase.Controllers
{
    public class ShoppingCartController : Controller
    {
        private Model1 db = new Model1();
        // GET: ShoppingCart
        Model1 database = new Model1();
        public ActionResult Index()
        {
            if (Session["Cart"] == null)
            {
                return View();
            }
            Cart cart = Session["Cart"] as Cart;

            int _id = (int?)Session["UserId"] ?? -1;
            if (_id > -1)
            {
                Customer _cus = database.Customers.FirstOrDefault(x => x.IDCus == _id);
                TempData["Address"] = _cus.AddressName;
            }
            return View(cart);
        }

        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult AddToCart(int id)
        {
            var _pro = database.OrderPro.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                GetCart().AddProductCart(_pro);
            }
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.RemoveCartItem(id);
            return RedirectToAction("Index", "ShoppingCart");
        }

        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.TotalQuantity();

            ViewBag.QuantityCart = total_quantity_item;
            return PartialView("BagCart");
        }

        public ActionResult UpdateCartQuantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.UpdateQuantity(id_pro, _quantity);
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                int _userId = (int)Session["UserId"];
                var _user = database.Customers.FirstOrDefault(x => x.IDCus == _userId);

                if (cart.Items.Count() == 0)
                {
                    return RedirectToAction("Index");
                }

                var p = form["AddressDelivery"];

                if (form["AddressDelivery"] == "")
                {
                    if (_user.AddressName == null || _user.AddressName == "")
                    {
                        TempData["Error"] = "Delivery address required";
                        return RedirectToAction("Index");
                    }
                    form["AddressDelivery"] = _user.AddressName;
                }

                if (form["CodeCustomer"] == null)
                {
                    form["CodeCustomer"] = _user.IDCus.ToString();

                }

                if (_user.AddressName == null)
                {
                    _user.AddressName = form["AddressDelivery"];
                    database.Entry<Customer>(_user).State = EntityState.Modified;
                }

                OrderPro _order = new OrderPro(); //Bang Hoa Don San pham

                _order.DateOrder = DateTime.Now;
                _order.PhoneNumber = int.Parse(form["PhoneNumber"]);
                _order.NameCus = form["NameCus"];
                _order.AddressDelivery = form["AddressDelivery"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);

                decimal totalDiscount = 0;
                decimal totalPrice = 0;
                decimal totalTax = 0;
                int totalQuantity = 0;

                foreach (var item in cart.Items)
                {
                    var prodTotal = (item._quantity * item._product.Price);
                    var tax = prodTotal * item._product.Tax;
                    var discount = item._product.Discount;
                    if (discount >= 0 && discount <= 1)
                    {
                        discount = prodTotal * discount;
                    }

                    OrderDetail _order_detail = new OrderDetail
                    {
                        IDProduct = item._product.ProductID,
                        IDOrder = _order.ID,

                        Quantity = item._quantity,
                        UnitPrice = item._product.Price,
                        Total = prodTotal - discount + tax,
                        Discount = item._product.Discount,
                        Tax = item._product.Tax,
                    };

                    totalQuantity += item._quantity;
                    totalDiscount += _order_detail.Discount;
                    totalPrice += _order_detail.Total;
                    totalTax += _order_detail.Tax;
                    database.OrderDetails.Add(_order_detail);

                    var _prod = database.OrderPro.Find(item._product.ProductID);
                    _prod.InvQuantity = Math.Max(_prod.InvQuantity - item._quantity, 0);
                    database.Entry(_prod).State = EntityState.Modified;
                }

                _order.TotalMoney = totalPrice;
                _order.TotalTax = totalTax;
                _order.TotalDiscount = totalDiscount;
                _order.TotalAmount = totalQuantity;

                database.OrderProes.Add(_order);
                database.SaveChanges();
                cart.ClearCart();
                return View("CheckOutSuccess", _order);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult BuyNow(int id)
        {
            Cart cart = GetCart();
            var _pro = database.OrderPro.SingleOrDefault(s => s.ProductID == id);
            if (_pro != null)
            {
                cart.AddProductCart(_pro);
                if (Session["UserId"] == null)
                {
                    TempData["Error"] = "You need to login first";
                    return RedirectToAction("Index");
                }
                int _userId = (int)Session["UserId"];
                var _user = database.Customers.FirstOrDefault(x => x.IDCus == _userId);
                FormCollection form = new FormCollection();
                form["AddressDelivery"] = _user.AddressName;
                form["PhoneNumber"] = _user.PhoneCus;
                form["CodeCustomer"] = _user.IDCus.ToString();
                form["NameCus"] = _user.NameCus;
                return RedirectToAction("CheckOut", "ShoppingCart", form);
            }
            TempData["Error"] = "Product doesn't exists or has been deleted";
            return RedirectToAction("Index", "Products");
        }

        public ActionResult CheckOutSuccess(OrderPro _order)
        {
            return View(_order);
        }

        public ActionResult CheckOrder(int? id)
        {
            if (id == null)
            {
                return View();
            }
            OrderPro _order = database.OrderProes.Where(x => x.ID == id).FirstOrDefault();
            if (_order == null)
            {
                return View();
            }
            //List<OrderDetail> order_items = new List<OrderDetail>();

            Product product;
            List<CartItem> cartItems = new List<CartItem>();
            var order_items = database.OrderDetails.Where(x => x.IDOrder == _order.ID).ToList();
            foreach (var item in order_items)
            {
                product = database.OrderPro.Where(x => x.ProductID == item.ID).FirstOrDefault();
                CartItem cartItem = new CartItem { _quantity = (int)item.Quantity, _product = item.Product };
                cartItems.Add(cartItem);
            }
            return View(cartItems);
        }
    }

}