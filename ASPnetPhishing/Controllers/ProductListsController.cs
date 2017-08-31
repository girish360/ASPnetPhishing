﻿using System;
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
            var products = db.Products.Include(p => p.ProductCategory).ToList();
            var category = db.ProductCategories.ToList();
            ViewBag.categoryModel = category;
            return View(products);
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
            if (Session["Cart"] == null)
            {
                currentCart = new Cart();
            }
            else
            {
                currentCart = (Cart)Session["Cart"];
            }
            
            if (Session["ProductType"] != null && Session["ProductType"].ToString().Equals("Permit"))
            {
                Product product = (Product)Session["Permit"];
                Product permit = db.Products.Find(product.Id);

                item = new LineItem();
                item.Qty = 1;
                item.Product = permit;
                currentCart.AddItem(item);
                Session["ProductType"] = "";
            }
            else
            {
                
                int qty;
                Product product = db.Products.Find(SelectedProduct.Id);
               


                if (SelectedProduct.Id != 0)
                {
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
                }

                
            }
            Session["Cart"] = currentCart;
            return View(currentCart);
        }

        public ActionResult UpdateQty(LineItem item)
        {
            Cart currentCart = (Cart) Session["Cart"];
            item.Product = db.Products.Find(item.Product.Id);
            currentCart.UpdateItem(item);
            Session["Cart"] = currentCart;
            return RedirectToAction("Cart");
        }

        public ActionResult RemoveItem(LineItem item)
        {
            Cart currentCart = (Cart)Session["Cart"];
            currentCart.RemoveItem(item);
            Session["Cart"] = currentCart;
            return RedirectToAction("Cart");
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
