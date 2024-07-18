using codeweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.UI;

namespace codeweb.Controllers
{
    public class HomeController : Controller
    {
        private Model1 database = new Model1();
        public ActionResult Index()
        {
            IEnumerable<Product> productList = database.OrderPro.OrderByDescending(x => x.NamePro).ToList();
            IEnumerable<Brand> BrandsList = database.Brands.OrderByDescending(x => x.BrandName).ToList();
            var tuple = new Tuple<IEnumerable<Product>, IEnumerable<Brand>>(productList, BrandsList);
            return View(tuple);
        }

        private void SearchInBrand(string query, ref List<Product> searchQuery)
        {
            string norm_name;
            foreach (var item in database.Brands)
            {
                norm_name = NormalizeDiacriticalCharacters(item.BrandName);
                if (norm_name.Contains(query))
                {
                    foreach (var prod in item.Products)
                    {
                        if (!searchQuery.Contains(prod))
                        {
                            searchQuery.Add(prod);
                        }
                    }
                }
            }
        }

        public ActionResult Search(string query, int? page = 1)
        {
            if (query == null || query.Length == 0)
            {
                ViewBag.MaxPage = 0;
                ViewBag.CurrentPage = 0;
                return View();
            }
            query = NormalizeDiacriticalCharacters(query);
            List<Product> searchQuery = new List<Product>();

            string norm_name;
            foreach (var item in database.OrderPro)
            {
                norm_name = NormalizeDiacriticalCharacters(item.NamePro);
                if (norm_name.Contains(query))
                {
                    searchQuery.Add(item);
                }
            }
            SearchInBrand(query, ref searchQuery);

            int maxPage = Math.Max(1, searchQuery.Count() / 10);
            if (page > maxPage)
            {
                page = maxPage;
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.CurrentPage = page;
            return View(searchQuery.Skip(((int)page - 1) * 10).Take(10));
        }

        private string NormalizeDiacriticalCharacters(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var normalised = value.Normalize(NormalizationForm.FormD).ToLower().ToCharArray();

            return new string(normalised.Where(c => (int)c <= 127).ToArray());
        }

        public ActionResult ProductDetails()
        {
            return View();
        }
    }
}