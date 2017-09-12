using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPnetPhishing.Models;

namespace ASPnetPhishing.Controllers.AdminControllers
{
    public class InvoicesController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: Invoices
        public ActionResult Index()
        {
            return View(db.vwInvoices.ToList());
        }

        // POST: query Invoices table
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? invoiceId)
        {
            vwInvoice invoice = null;
            if  (invoiceId != null)
            {
                invoice = db.vwInvoices.Where(v => v.Invoice_Number == invoiceId).SingleOrDefault();
                if  (invoice != null)
                {
                    return View(db.vwInvoices.Where(v => v.Invoice_Number == invoiceId).ToList());
                }
                else
                {
                    return View(db.vwInvoices.ToList());
                }
            }
            else
            {
                return View(db.vwInvoices.ToList());
            }
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vwInvoice vwInvoice = db.vwInvoices.Where(vwi => vwi.Invoice_Number == id).SingleOrDefault();
            if (vwInvoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.LineItems = db.LineItems.Where(li => li.InvoiceId == id).ToList();
            return View(vwInvoice);
        }

        // GET: Invoices/Edit
        public ActionResult Edit(int? id)
        {
            Invoice invoice = null;
            if (id == null)
            {
                if (Session["EditInvoice"] == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    invoice = (Invoice)Session["EditInvoice"];
                }
            }
            else
            {
                invoice = db.Invoices.Find(id);
                Session["EditInvoice"] = invoice;
                
            }
            var cards = db.CardRecords.Where(cr => cr.CustomerId == invoice.UserID).ToList();
            ViewBag.Card = new SelectList(cards, "Id", "CardNumber");
            var shippingAddresses = db.Shippings.Where(s => s.CustomerId == invoice.UserID).ToList();
            ViewBag.Shipping = new SelectList(shippingAddresses, "Id", "ShippingAddress", invoice.ShippingId);
            ViewBag.LineItems = db.LineItems.Where(item => item.InvoiceId == invoice.Id).ToList();
            return View(invoice);
        }

        // POST: Invoices/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Invoice invoice)
        {
            db.Invoices.Find(invoice.Id).ShippingId = invoice.ShippingId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Invoices/EditPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPayment(PaymentRecord pr, Invoice invoice)
        {
            db.PaymentRecords.Find(pr.PaymentId).CardRecordId = pr.CardRecordId;
            db.SaveChanges();
            Session["EditInvoice"] = db.Invoices.Find(invoice.Id);
            return RedirectToAction("Edit");
        }

        // GET: Invoices/AddLineItem
        public ActionResult AddLineItem(int? id, int? qty)
        {
            if (qty == null && id == null)
            {
                var products = db.Products.ToList();
                return View(products);
            }
            else
            {
                Invoice invoice = (Invoice)Session["EditInvoice"];
                LineItem li = new LineItem();
                li.InvoiceId = invoice.Id;
                li.Qty = Convert.ToInt32(qty);
                li.ProductId = Convert.ToInt32(id);
                li.Product = db.Products.Find(li.ProductId);

                // save lineitem
                db.LineItems.Add(li);
                db.SaveChanges();

                // modify and save invoice
                List<LineItem> lis = db.LineItems.Where(litem => litem.InvoiceId == invoice.Id).ToList();
                decimal total = 0m;
                foreach (LineItem line in lis)
                {
                    total += line.LineTotal;
                }
                db.Invoices.Find(invoice.Id).Total = total * (1 + Cart.TAX);
                db.SaveChanges();

                // modify and save payment
                db.PaymentRecords.Find(invoice.PaymentId).PaymentAmount = total * (1 + Cart.TAX);
                db.SaveChanges();

                Session["EditInvoice"] = db.Invoices.Find(invoice.Id);
                return RedirectToAction("Edit");
            }
        }

        // POST: Invoices/AddLineItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLineItem(string productName)
        {
            var products = (from p in db.Products
                            where p.Name.Contains(productName)
                            select p).ToList();
            return View(products);
        }

        // GET: Invoices/DeleteLineItem
        public ActionResult DeleteLineItem(int id)
        {
            Invoice invoice = (Invoice)Session["EditInvoice"];

            // remove item
            db.LineItems.Remove(db.LineItems.Find(id));
            db.SaveChanges();

            // modify and save invoice
            List<LineItem> lis = db.LineItems.Where(litem => litem.InvoiceId == invoice.Id).ToList();
            decimal total = 0m;
            foreach (LineItem line in lis)
            {
                total += line.LineTotal;
            }
            db.Invoices.Find(invoice.Id).Total = total * (1 + Cart.TAX);
            db.SaveChanges();

            // modify and save payment
            db.PaymentRecords.Find(invoice.PaymentId).PaymentAmount = total * (1 + Cart.TAX);
            db.SaveChanges();

            Session["EditInvoice"] = db.Invoices.Find(invoice.Id);
            return RedirectToAction("Edit");
        }

        //// GET: Invoices/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    vwInvoice vwInvoice = db.vwInvoices.Find(id);
        //    if (vwInvoice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(vwInvoice);
        //}

        //// POST: Invoices/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    vwInvoice vwInvoice = db.vwInvoices.Find(id);
        //    db.vwInvoices.Remove(vwInvoice);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
