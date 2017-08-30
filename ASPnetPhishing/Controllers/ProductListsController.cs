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
    public class ProductListsController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: ProductLists
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductCategory);
            return View(products.ToList());
        }

        // GET: ProductLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Cart(int? qty, Product SelectedProduct, Cart currentCart)
        {
            if (currentCart == null)
            {
                currentCart = new Cart();
            }
            if (qty == null)
            {
                qty = 1;
            }
            Product product = db.Products.Find(SelectedProduct.Id);
            LineItem currentItem = new LineItem();
            currentItem.Qty = Convert.ToInt32(qty);
            currentItem.Product = product;
            currentItem.SetLineTotal();
            currentCart.AddItem(currentItem);
            return View(currentCart);
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
