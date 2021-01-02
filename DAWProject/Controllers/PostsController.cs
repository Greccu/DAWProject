using DAWProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWProject.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index(int? id,string searching,string sortOrder)
        {
         
                var posts = from po in db.Posts.Include("User").Include("Category")
                            where ((id == null)||(po.CategoryId == id)) && (searching == null || po.Content.Contains(searching) || po.Title.Contains(searching) || po.Comments.Any(c => c.Content.Contains(searching))) 
                            select po;
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.Message = TempData["message"];
                }
                switch (sortOrder)
                {
                    case "old":
                        posts = posts.OrderBy(p => p.CreatedAt);
                        break;
                    case "categ":
                        posts = posts.OrderBy(p => p.Category.CategoryName);
                        break;
                    case "popular":
                        posts = posts.OrderByDescending(p => p.Comments.Count());
                        break;
                    default:
                        posts = posts.OrderByDescending(p => p.CreatedAt);
                        break;

                }
                ViewBag.Posts = posts;
                ViewBag.Count = 0;
                ViewBag.Category = null;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.CurrentSearch = searching;
                if (id!=null)
                {
                    ViewBag.Category = db.Categories.Find(id);
                    ViewBag.CategoryId = ViewBag.Category.CategoryId;
                }
                else
                {
                    ViewBag.CategoryId = "all";
                }
           
            return View();
        }



        // GET implicit - Vizualizarea unei postari
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            try
           {
                SetAccessRights();
                //ViewBag.Post = db.Posts.Include("Category").Find(id);
                Post post = db.Posts.Find(id);;
                return View(post);
            }
            catch(Exception e)
            {
                SetAccessRights();
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(Comment comm)
        {
            comm.UserId = User.Identity.GetUserId();
            comm.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Comments.Add(comm);
                    db.SaveChanges();
                    return Redirect("/Posts/Show/" + comm.PostId);
                }

                else
                {
                    SetAccessRights();
                    Post p = db.Posts.Find(comm.PostId);
                    return View(p);
                }

            }

            catch (Exception)
            {
                Post p = db.Posts.Find(comm.PostId);
                SetAccessRights();

                return View(p);
            }

        }

        // CREATE
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New()
        {
            Post post = new Post
            {
                //preluam lista de categorii
                Categ = GetAllCategories(),

                UserId = User.Identity.GetUserId()
            };
            return View(post);
        }
  
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(Post post)
        {
            post.UserId = User.Identity.GetUserId();
            post.CreatedAt = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "Post Added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    post.Categ = GetAllCategories();
                    return View(post);
                }
            }
            catch (Exception)
            {
                //ViewBag.ErrorMessage = e.Message;
                post.Categ = GetAllCategories();
                return View(post);
            }
        }

        // GET implicit: Afisarea datelor unei postari pentru editare
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            post.Categ = GetAllCategories();
            return View(post);
        }

        [HttpPut]
        public ActionResult Edit(int id, Post requestPost)
        {
            requestPost.Categ = GetAllCategories();
            
            try
            {
                if (ModelState.IsValid)
                {
                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId() ||
User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(post))
                        {
                            post.Title = requestPost.Title;
                            post.Content = requestPost.Content;
                            post.CreatedAt = requestPost.CreatedAt;
                            post.CategoryId = requestPost.CategoryId;
                            db.SaveChanges();
                            TempData["message"] = "Post edit completed!";
                        }
                        return RedirectToAction("Show/" + id.ToString());
                    }
                    else
                    {
                        TempData["message"] = "You can't edit a post that isn't yours!";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    requestPost.Categ = GetAllCategories();
                    return View(requestPost);
                }
            }
            catch (Exception)
            {
                requestPost.Categ = GetAllCategories();
                return View(requestPost);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);

            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Post deleted!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You can't delete a post that isn't yours!";
                return RedirectToAction("Index");
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

        private void SetAccessRights()
        {
            ViewBag.showButtons = false;
            if (User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                ViewBag.showButtons = true;
            }

            ViewBag.isAdmin = User.IsInRole("Admin");
            ViewBag.currentUser = User.Identity.GetUserId();
        }

    }
}