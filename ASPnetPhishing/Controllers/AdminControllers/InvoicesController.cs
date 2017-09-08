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
            return View(vwInvoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vwInvoice vwInvoice = db.vwInvoices.Find(id);
            if (vwInvoice == null)
            {
                return HttpNotFound();
            }
            return View(vwInvoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            vwInvoice vwInvoice = db.vwInvoices.Find(id);
            db.vwInvoices.Remove(vwInvoice);
            db.SaveChanges();
            return RedirectToAction("Index");
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
