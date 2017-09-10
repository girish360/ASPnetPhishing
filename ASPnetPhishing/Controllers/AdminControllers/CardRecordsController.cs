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
    public class CardRecordsController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: CardRecords
        public ActionResult Index()
        {
            var cardRecords = db.CardRecords.Include(c => c.AspNetUser);
            return View(cardRecords.ToList());
        }

        // GET: CardRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardRecord cardRecord = db.CardRecords.Find(id);
            if (cardRecord == null)
            {
                return HttpNotFound();
            }
            return View(cardRecord);
        }

        // GET: CardRecords/SelectUser
        public ActionResult SelectUser(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Create", new { customerId = id });
            }
        }

        // POST: CardRecords/SelectUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectUser(string userEmail)
        {
            var selecteduser = (from u in db.AspNetUsers
                                where u.Email.Contains(userEmail)
                                select u).ToList();
            return View(selecteduser);
        }

        // GET: CardRecords/Create
        public ActionResult Create(string customerId)
        {
            if (customerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var cardRecord = new CardRecord();
                cardRecord.CustomerId = customerId;
                cardRecord.AspNetUser = db.AspNetUsers.Find(cardRecord.CustomerId);
                return View(cardRecord);
            }
        }

        // POST: CardRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,CardNumber,CCV,ExpDate,BillingAddress,BillingCity,BillingState,BillingZip,BillingEmail")] CardRecord cardRecord)
        {
            if (ModelState.IsValid)
            {
                db.CardRecords.Add(cardRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", cardRecord.CustomerId);
            return View(cardRecord);
        }

        // GET: CardRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardRecord cardRecord = db.CardRecords.Find(id);
            if (cardRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", cardRecord.CustomerId);
            return View(cardRecord);
        }

        // POST: CardRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,CardNumber,CCV,ExpDate,BillingAddress,BillingCity,BillingState,BillingZip,BillingEmail")] CardRecord cardRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cardRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", cardRecord.CustomerId);
            return View(cardRecord);
        }

        // GET: CardRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardRecord cardRecord = db.CardRecords.Find(id);
            if (cardRecord == null)
            {
                return HttpNotFound();
            }
            return View(cardRecord);
        }

        // POST: CardRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CardRecord cardRecord = db.CardRecords.Find(id);
            db.CardRecords.Remove(cardRecord);
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
