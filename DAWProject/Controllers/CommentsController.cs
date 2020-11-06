using DAWProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWProject.Controllers
{
    public class CommentsController : Controller
    {
        
        private Models.AppContext db = new Models.AppContext();

        // Show all comments (only available to the administrator)
        public ActionResult Index()
        {
            var comments = db.Comments;
            ViewBag.Comments = comments;
            return View();
        }

        [HttpPost]
        public ActionResult New(Comment comment)
        {
            comment.Date = DateTime.Now;
            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                TempData["message"] = "Comment Added!";
                return RedirectToAction("Show","Posts", new {id = comment.PostId });
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(comment);
            }
        }

        // GET implicit: Afisarea datelor unei postari pentru editare
        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            return View(comment);
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comment = db.Comments.Find(id);
                if (TryUpdateModel(comment))
                {
                    comment.Content = requestComment.Content;
                    comment.Date = requestComment.Date;
                    db.SaveChanges();
                    TempData["message"] = "Comment edit completed!";
                    return RedirectToAction("Show", "Posts", new { id = comment.PostId });
                }
                return View(requestComment);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(requestComment);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {

                Comment comment = db.Comments.Find(id);
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["message"] = "Comment deleted!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

        }




    }
}
