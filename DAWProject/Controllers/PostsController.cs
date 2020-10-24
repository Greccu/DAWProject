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
        private PostDBContext db = new PostDBContext();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include("Category");
            ViewBag.Posts = posts;
            return View();
        }
        
        // GET implicit - Vizualizarea unei postari
        public ActionResult Show(int id)
        {
            //try
           // {
                //ViewBag.Post = db.Posts.Include("Category").Find(id);
                ViewBag.Post = db.Posts.Include("Category").First(p => p.post_id == id);
                return View();
            //}
            //catch(Exception e)
            //{
                //ViewBag.ErrorMessage = e.Message;
                //return View("Error");
            //}
        }

        // CREATE
        public ActionResult New()
        {
            var categories = from category in db.Categories
                             select category;
            ViewBag.Categories = categories;
            return View();
        }
  
        [HttpPost]
        public ActionResult New(Post post)
        {
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        // GET implicit: Afisarea datelor unei postari pentru editare
        public ActionResult Edit(int id)
        {
            //ViewBag.Post = db.Posts.Include("Category").Find(id);
            ViewBag.Post = db.Posts.Include("Category").First(p => p.post_id == id);
            var categories = from category in db.Categories
                             select category;
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id,Post requestpost)
        {
            try
            {
                //Post post = db.Posts.Include("Category").Find(id);
                Post post = db.Posts.Include("Category").First(p => p.post_id == id);
                if (TryUpdateModel(post))
                {
                    post.title = requestpost.title;
                    post.content = requestpost.content;
                    post.category_id = requestpost.category_id;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
           
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                //Post post = db.Posts.Include("Category").Find(id);
                Post post = db.Posts.Include("Category").First(p => p.post_id == id);
                db.Posts.Remove(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }

        }
    }
}