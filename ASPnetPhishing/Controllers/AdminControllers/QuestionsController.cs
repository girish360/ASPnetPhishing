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
    [Authorize(Roles ="Employee, Manager, Admin")]
    public class QuestionsController : Controller
    {
        private AdminConnection db = new AdminConnection();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Where(q => q.IsAnswered == false).Include(q => q.AspNetUser);
            return View(questions.ToList());
        }

        // GET: Question/Answer
        public ActionResult Answer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Question quest = db.Questions.Find(id);
                return View(quest);
            }
            
        }

        // POST: Question/Answer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Answer([Bind(Include ="Id, Answer")] Question question)
        {
            if (question == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                db.Questions.Find(question.Id).Answer = question.Answer;
                db.Questions.Find(question.Id).IsAnswered = true;
                db.SaveChanges();
                return RedirectToAction("Index", "Questions");
            }
        }

        // GET: Question/CreateNew
        [AllowAnonymous]
        public ActionResult CreateNew()
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Question q = new Question();
                q.CustomerId = User.Identity.GetUserId();
                q.IsAnswered = false;
                return View(q);
            }            
        }

        //POST: Question/CreateNew
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateNew([Bind(Include ="CustomerId, Question1")] Question q)
        {
            q.AspNetUser = db.AspNetUsers.Find(q.CustomerId);
            db.Questions.Add(q);
            db.SaveChanges();

            ViewBag.QuestionConfirm = "Thank you for your interest in us. We will reply your question as soon as possible.";
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
