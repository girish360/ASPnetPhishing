using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPnetPhishing.Models;

namespace ASPnetPhishing.Controllers
{
    public class PermitController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: Permit
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductCategory);
            return View(products.ToList());
        }

        // GET: Permit/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        public ActionResult Cart(Product product)
        {
            Session["Permit"] = product;
            Session["ProductType"] = "Permit";
            return RedirectToAction("Cart", "ProductLists");
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
