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
    public class PaymentsController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: Payments
        public ActionResult Index()
        {
            var paymentRecords = db.PaymentRecords.Include(p => p.CardRecord);
            return View(paymentRecords.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentRecord paymentRecord = db.PaymentRecords.Find(id);
            if (paymentRecord == null)
            {
                return HttpNotFound();
            }
            return View(paymentRecord);
        }

        // GET: Select Invoice
        public ActionResult SelectInvoice()
        {
            return View(db.vwInvoices.ToList());
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            var cardInfo = (from c in db.CardRecords
                            join anu in db.AspNetUsers
                            on c.CustomerId equals anu.Id
                            select new { Id = c.Id, CardInfo = anu.Email + " -> " + c.CardNumber });
            ViewBag.CardNumber = new SelectList(cardInfo, "Id", "CardInfo");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentId,CardRecordId,PaymentAmount")] PaymentRecord paymentRecord)
        {
            if (ModelState.IsValid)
            {
                db.PaymentRecords.Add(paymentRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardRecordId = new SelectList(db.CardRecords, "Id", "CustomerId", paymentRecord.CardRecordId);
            return View(paymentRecord);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentRecord paymentRecord = db.PaymentRecords.Find(id);
            if (paymentRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardRecordId = new SelectList(db.CardRecords, "Id", "CustomerId", paymentRecord.CardRecordId);
            return View(paymentRecord);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaymentId,CardRecordId,PaymentAmount")] PaymentRecord paymentRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CardRecordId = new SelectList(db.CardRecords, "Id", "CustomerId", paymentRecord.CardRecordId);
            return View(paymentRecord);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentRecord paymentRecord = db.PaymentRecords.Find(id);
            if (paymentRecord == null)
            {
                return HttpNotFound();
            }
            return View(paymentRecord);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentRecord paymentRecord = db.PaymentRecords.Find(id);
            db.PaymentRecords.Remove(paymentRecord);
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
