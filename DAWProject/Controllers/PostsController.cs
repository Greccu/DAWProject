using DAWProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWProject.Controllers
{
    public class PostsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();


        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var posts = db.Posts.Include("Category");
                ViewBag.Posts = posts;
            }
            else
            {
                var posts = from po in db.Posts
                            where po.CategoryId == id
                            select po;
                ViewBag.Category = db.Categories.Find(id);
                ViewBag.Posts = posts;
            }
            return View();
        }

        // GET implicit - Vizualizarea unei postari
        public ActionResult Show(int id)
        {
            try
           {
                //ViewBag.Post = db.Posts.Include("Category").Find(id);
                Post post = db.Posts.Find(id);;
                return View(post);
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        // CREATE
        public ActionResult New()
        {
            Post post = new Post
            {
                //preluam lista de categorii
                Categ = GetAllCategories()
            };
            return View(post);
        }
  
        [HttpPost]
        public ActionResult New(Post post)
        {
            post.CreatedAt = DateTime.Now;
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                TempData["message"] = "Post Added!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(post);
            }
        }

        // GET implicit: Afisarea datelor unei postari pentru editare
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            post.Categ = GetAllCategories();
            return View(post);
        }

        [HttpPut]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                Post post = db.Posts.Find(id);
                if (TryUpdateModel(post))
                {
                    post.Title = requestPost.Title;
                    post.Content = requestPost.Content;
                    post.CreatedAt = requestPost.CreatedAt;
                    post.CategoryId = requestPost.CategoryId;
                    db.SaveChanges();
                    TempData["message"] = "Post edit completed!";
                    return RedirectToAction("Index");
                }
                return View(requestPost);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(requestPost);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {

                Post post = db.Posts.Find(id);
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Post deleted!";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

    }
}