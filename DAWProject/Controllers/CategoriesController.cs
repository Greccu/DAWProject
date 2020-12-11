using DAWProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DAWProject.Controllers
{
    [Authorize(Roles = "Admin,Editor,User")]
    public class CategoriesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index()
        {
            var categories = db.Categories;
            ViewBag.Categories = categories;
            return View();
        }


        // CREATE
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Category cat)
        {
            try
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                TempData["message"] = "Category added!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            //ViewBag.Post = db.Posts.Include("Category").Find(id);
            ViewBag.Category = db.Categories.First(c => c.CategoryId == id);
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Category requestcat)
        {
            try
            {
                //Post post = db.Posts.Include("Category").Find(id);
                Category category = db.Categories.First(c => c.CategoryId == id);
                if (TryUpdateModel(category))
                {
                    category.CategoryName = requestcat.CategoryName;
                    db.SaveChanges();
                    TempData["message"] = "Category edited";
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
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
                Category category = db.Categories.First(c => c.CategoryId == id);
                db.Categories.Remove(category);
                db.SaveChanges();
                TempData["message"] = "Category deleted!";
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