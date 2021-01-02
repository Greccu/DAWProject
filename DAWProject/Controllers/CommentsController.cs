using DAWProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DAWProject.Controllers
{
    public class CommentsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

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
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);

            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(comm);
            }
            else
            {
                TempData["message"] = "You don't have the rights to do this!";
                return RedirectToAction("Show", "Posts", new { id = comm.PostId });
            }

        }


        [HttpPut]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comm = db.Comments.Find(id);

                if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(comm))
                    {
                        comm.Content = requestComment.Content;
                        comm.Date = requestComment.Date;
                        db.SaveChanges();
                        TempData["message"] = "Comment edit completed!";
                        return RedirectToAction("Show", "Posts", new { id = comm.PostId });
                    }
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                else
                {
                    TempData["message"] = "You don't have the rights to do this!";
                    return RedirectToAction("Show", "Posts", new { id = comm.PostId });
                }
            }
            catch (Exception)
            {
                return View(requestComment);
            }
        }


        [HttpDelete]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                TempData["message"] = "Comment deleted!";
                return Redirect("/Posts/Show/" + comm.PostId);
            }
            else
            {
                TempData["message"] = "You don't have the rights to do this!";
                return RedirectToAction("Show", "Posts", new { id = comm.PostId });
            }

        }




    }
}
