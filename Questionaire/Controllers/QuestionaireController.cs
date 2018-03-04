using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Questionaire.Models;
using PagedList;

namespace Questionaire.Controllers
{
    [Authorize]
    public class QuestionaireController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DateTime MyDate = DateTime.Now;
        // GET: Questionaire
        public ActionResult Index(int? page)
        {
            var model = db.Questions
                .Include(q => q.QuestionOptions)
                .Where(q=>q.AvailableFrom <= MyDate)
                .Where(q=> DbFunctions.AddDays(q.AvailableTo, 1) > MyDate)
                .ToList();

            var userAnswersOld = db.UserAnswers
                .Where(a => a.Question.AvailableFrom <= MyDate)
                .Where(a => DbFunctions.AddDays(a.Question.AvailableTo, 1) > MyDate)
                .Where(a => a.UserName == User.Identity.Name)
                .ToList();
            ViewBag.userAnswersOld = userAnswersOld;

            //return View(model);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult SaveAnswer(UserAnswer answer)
        {
            var userAnswer_old = db.UserAnswers
                .Where(a => a.Question.AvailableFrom <= MyDate)
                .Where(a => DbFunctions.AddDays(a.Question.AvailableTo, 1) > MyDate)
                .Where(a => a.UserName == User.Identity.Name)
                .Where(a => a.QuestionID == answer.QuestionID).ToList();
            //do something
            if (ModelState.IsValid)
            {
                
                if (userAnswer_old.Count == 0)
                {
                    db.Entry(answer).State = answer.UserAnswerID == 0 ?
                                EntityState.Added :
                                EntityState.Modified;

                    db.SaveChanges();
                    return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }else if(userAnswer_old[0].IsFinalSubmission == false)
                {
                    userAnswer_old[0].QuestionOptionID=answer.QuestionOptionID;
                    userAnswer_old[0].AnswerDate = answer.AnswerDate;
                    userAnswer_old[0].IsFinalSubmission = answer.IsFinalSubmission;
                    db.Entry(userAnswer_old[0]).State = EntityState.Modified;

                    db.SaveChanges();
                    return Json(new { Result = "Success", Message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = "UnSuccessfull", Message = "Final Submission done already" }, JsonRequestBehavior.AllowGet);
                }
                
            }

            return Json(new { Result = "UnSuccessfull", Message = "Invalid Model" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Result()
        {
            //var model = db.Questions
            //    .Include(q => q.QuestionOptions)
            //    .Where(q => q.AvailableFrom <= MyDate)
            //    .Where(q => DbFunctions.AddDays(q.AvailableTo, 1) > MyDate)
            //    .ToList();
            var userAnswers = db.UserAnswers
                .Include(a=>a.Question)
                .Include(a=>a.QuestionOption)
                .Where(a => a.Question.AvailableFrom <= MyDate)
                .Where(a => DbFunctions.AddDays(a.Question.AvailableTo, 1) > MyDate)
                .Where(a => a.UserName == User.Identity.Name)
                .ToList();

            return View(userAnswers);
        }
        // GET: Questionaire/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

       

        // GET: Questionaire/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questionaire/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionID,QuestionTitle,AvailableFrom,AvailableTo")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(question);
        }

        // GET: Questionaire/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questionaire/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionID,QuestionTitle,AvailableFrom,AvailableTo")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questionaire/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questionaire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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
