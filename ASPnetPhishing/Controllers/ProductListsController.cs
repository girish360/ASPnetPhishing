﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPnetPhishing.Models;
using Microsoft.AspNet.Identity;

namespace ASPnetPhishing.Controllers
{
    public class ProductListsController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: ProductLists
        public ActionResult Index()
        {
            var category = db.ProductCategories.Where(pc => pc.CategoryName != "Permit").ToList();
            return View(category);
        }

        // GET: ProductLists/show queried products
        public ActionResult ProductList(ProductCategory productCategory)
        {
            int catId = productCategory.Id;
            List<ASPnetPhishing.Models.Product> products;
            if (catId >= 0)
            {
                if (catId == 0 && Session["category"] != null)
                {
                    catId = Convert.ToInt32(Session["category"]);
                }
                else if (catId == 0 && Session["category"] == null)
                {
                    return RedirectToAction("ProductList", new { id = -1 });
                }
                Session["category"] = catId;
                products = db.Products.Where(p => p.CategoryId == catId).Include(p => p.ProductCategory).ToList();
            }
            else
            {
                products = db.Products.Include(p => p.ProductCategory).ToList();
            }
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

        // GET: ProductLists/Cart
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

            // check to see if just to view the cart
            if(Session["ViewCart"] == null)
            {
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
            }
            else if (Session["ViewCart"].ToString().Equals("ViewCart"))
            {
                Session["ViewCart"] = null;
            }
            
            return View(currentCart);
        }

        public ActionResult ViewCart()
        {
            Session["ViewCart"] = "ViewCart";
            return RedirectToAction("Cart");
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

        public ActionResult CheckOut()
        {
            if (User.Identity.GetUserId() != null)
            { 
                Cart cartToCheckout = (Cart) Session["Cart"];
                Invoice currentInvoice = null;
                if (Session["CurrentInvoice"] != null)
                {
                    currentInvoice = (Invoice) Session["CurrentInvoice"];
                }
                else
                {
                    currentInvoice = new Invoice();
                }
            
                currentInvoice.DateTime = DateTime.Now;
                currentInvoice.UserID = User.Identity.GetUserId();
                foreach (LineItem li in cartToCheckout.CartItems)
                {
                    currentInvoice.LineItems.Add(li);
                }
                currentInvoice.Total = cartToCheckout.Total;
            
                ViewBag.Card = new SelectList(db.CardRecords.Where(cr => cr.CustomerId == currentInvoice.UserID).ToList(), "Id", "CardNumber");
                ViewBag.Shipping = new SelectList(db.Shippings.Where(s => s.CustomerId == currentInvoice.UserID).ToList(), "Id", "ShippingAddress");
                Session["CurrentInvoice"] = currentInvoice;
            
                return View(currentInvoice);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Confirmation(CardRecord cardRecord, Shipping shipping)
        {
            //Invoice currentInvoice = (Invoice)Session["CurrentInvoice"];
            //// save payment record
            //PaymentRecord pr = new PaymentRecord();
            //pr.CardRecordId = cardRecord.Id;
            //pr.PaymentAmount = Convert.ToDecimal(currentInvoice.Total);
            //db.PaymentRecords.Add(pr);
            //currentInvoice.PaymentId = pr.PaymentId;

            //// save invoice
            //db.Invoices.Add(currentInvoice);

            //// save line items
            //foreach (LineItem li in currentInvoice.LineItems)
            //{
            //    db.LineItems.Add(li);
            //}
            

            //db.SaveChanges();

            return View(/*currentInvoice*/);
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
