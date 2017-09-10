using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPnetPhishing.Models;
using System.Xml.Linq;

namespace ASPnetPhishing.Controllers.AdminControllers
{
    public class ShippingsController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: Shippings
        public ActionResult Index()
        {
            var shippings = db.Shippings.Include(s => s.AspNetUser);
            return View(shippings.ToList());
        }

        // GET: Shippings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }

        // GET Shippings/SelectUser
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

        // POST: Shippings/SelectUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectUser(string userEmail)
        {
            var selecteduser = (from u in db.AspNetUsers
                                where u.Email.Contains(userEmail)
                                select u).ToList();
            return View(selecteduser);
        }

        // GET: Shippings/Create
        public ActionResult Create(string customerId)
        {
            if (customerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                // prepare states collection for dropdown
                var model = XDocument.Load(Server.MapPath(Url.Content("~/App_Data/states.xml")));
                IEnumerable<XElement> result = from c in model.Elements("states").Elements("state") select c;

                var listItems = new List<SelectListItem>();

                listItems.Add(new SelectListItem
                {
                    Text = "--- Please select a State ---",
                    Value = "",
                });
                foreach (var xElement in result)
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = xElement.Attribute("name").Value,
                        Value = xElement.Attribute("abbreviation").Value,
                    });
                }
                SelectList selectList = new SelectList(listItems, "Value", "Text");
                ViewBag.States = selectList;

                var newShipping = new Shipping();
                newShipping.CustomerId = customerId;
                newShipping.AspNetUser = db.AspNetUsers.Find(customerId);
                return View(newShipping);
            }            
        }

        // POST: Shippings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,FirstName,LastName,ShippingAddress,ShippingCity,ShippingState,ShippingZipCode,ShippingPhone,ShippingEmail")] Shipping shipping)
        {
            if (ModelState.IsValid)
            {
                db.Shippings.Add(shipping);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", shipping.CustomerId);
            return View(shipping);
        }

        // GET: Shippings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            // prepare states collection for dropdown
            var model = XDocument.Load(Server.MapPath(Url.Content("~/App_Data/states.xml")));
            IEnumerable<XElement> result = from c in model.Elements("states").Elements("state") select c;

            var listItems = new List<SelectListItem>();

            listItems.Add(new SelectListItem
            {
                Text = "--- Please select a State ---",
                Value = "",
            });
            foreach (var xElement in result)
            {
                listItems.Add(new SelectListItem
                {
                    Text = xElement.Attribute("name").Value,
                    Value = xElement.Attribute("abbreviation").Value,
                });
            }

            SelectList selectList = new SelectList(listItems, "Value", "Text", shipping.ShippingState);

            ViewBag.States = selectList;
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", shipping.CustomerId);
            return View(shipping);
        }

        // POST: Shippings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,FirstName,LastName,ShippingAddress,ShippingCity,ShippingState,ShippingZipCode,ShippingPhone,ShippingEmail")] Shipping shipping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.AspNetUsers, "Id", "Email", shipping.CustomerId);
            return View(shipping);
        }

        // GET: Shippings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }

        // POST: Shippings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipping shipping = db.Shippings.Find(id);
            db.Shippings.Remove(shipping);
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
