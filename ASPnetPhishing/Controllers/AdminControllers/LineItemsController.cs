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
    public class LineItemsController : Controller
    {
        private Admin db = new Admin();

        // GET: LineItems
        public ActionResult Index()
        {
            var lineItems = db.LineItems.Include(l => l.Invoice).Include(l => l.Product);
            return View(lineItems.ToList());
        }

        // GET: LineItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineItem lineItem = db.LineItems.Find(id);
            if (lineItem == null)
            {
                return HttpNotFound();
            }
            return View(lineItem);
        }

        // GET: LineItems/Create
        public ActionResult Create()
        {
            ViewBag.InvoiceId = new SelectList(db.Invoices, "Id", "UserID");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: LineItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LineItemId,InvoiceId,ProductId,Qty,LineTotal")] LineItem lineItem)
        {
            if (ModelState.IsValid)
            {
                db.LineItems.Add(lineItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvoiceId = new SelectList(db.Invoices, "Id", "UserID", lineItem.InvoiceId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", lineItem.ProductId);
            return View(lineItem);
        }

        // GET: LineItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineItem lineItem = db.LineItems.Find(id);
            if (lineItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvoiceId = new SelectList(db.Invoices, "Id", "UserID", lineItem.InvoiceId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", lineItem.ProductId);
            return View(lineItem);
        }

        // POST: LineItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LineItemId,InvoiceId,ProductId,Qty,LineTotal")] LineItem lineItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvoiceId = new SelectList(db.Invoices, "Id", "UserID", lineItem.InvoiceId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", lineItem.ProductId);
            return View(lineItem);
        }

        // GET: LineItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineItem lineItem = db.LineItems.Find(id);
            if (lineItem == null)
            {
                return HttpNotFound();
            }
            return View(lineItem);
        }

        // POST: LineItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LineItem lineItem = db.LineItems.Find(id);
            db.LineItems.Remove(lineItem);
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
