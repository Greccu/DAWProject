using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWProject.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
    }
}