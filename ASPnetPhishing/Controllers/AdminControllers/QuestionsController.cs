using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPnetPhishing.Models;
using Microsoft.AspNet.Identity;

namespace ASPnetPhishing.Controllers.AdminControllers
{
    public class QuestionsController : Controller
    {
        private QuestionEntity db = new QuestionEntity();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Where(q => q.IsAnswered == false).Include(q => q.AspNetUser);
            return View(questions.ToList());
        }

        // POST: Question/Answer
        public ActionResult Answer(string answer, int? id)
        {
            if (answer == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                db.Questions.Find(id).Answer = answer;
                db.Questions.Find(id).IsAnswered = true;
                db.SaveChanges();
                return RedirectToAction("Index", "Questions"); 
            }
        }

        // GET: Question/CreateNew
        public ActionResult CreateNew()
        {
            Question q = new Question();
            q.CustomerId = User.Identity.GetUserId();
            q.IsAnswered = false;
            db.Questions.Add(q);
            return View(q);
        }

        //POST: Question/CreateNew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNew(Question q)
        {
            // code this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            return View();
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
