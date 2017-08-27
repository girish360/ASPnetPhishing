﻿using System;
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

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
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

        // GET: Invoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Invoice_Number,Invoice_Date,Invoice_Total,Customer_Email,Card_Number,Ship_To,City,State,Zip_Code,Phone,Email")] vwInvoice vwInvoice)
        {
            if (ModelState.IsValid)
            {
                db.vwInvoices.Add(vwInvoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vwInvoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Invoice_Number,Invoice_Date,Invoice_Total,Customer_Email,Card_Number,Ship_To,City,State,Zip_Code,Phone,Email")] vwInvoice vwInvoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vwInvoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
