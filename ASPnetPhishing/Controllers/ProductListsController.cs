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

        public ActionResult Cart(Product SelectedProduct, LineItem item)
        {
            Cart currentCart;
            int qty;
            Product product = db.Products.Find(SelectedProduct.Id);
            if (Session["Cart"] == null)
            {
                currentCart = new Cart();
            }
            else
            {
                currentCart = (Cart)Session["Cart"];
            }

            if (item.Qty == 0)
            {
                qty = 1;
            }
            else
            {
                qty = Convert.ToInt32(item.Qty);
            }

            item = new LineItem();
            item.Qty = qty;
            item.Product = product;
            currentCart.AddItem(item);
            Session["Cart"] = currentCart;
            return View(currentCart);
            
        }

        public void UpdateQty(LineItem item)
        {
            Cart currentCart = (Cart) Session["Cart"];
            item.Product = db.Products.Find(item.Product.Id);
            currentCart.UpdateItem(item);
            Session["Cart"] = currentCart;
            //Response.Redirect("~/Controllers/ProductListsController/Cart");
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
