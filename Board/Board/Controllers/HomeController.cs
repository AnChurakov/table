using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Board.Models;

namespace Board.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var selectCategorys = _context.Categorys.ToList();

            ViewBag.SubCategory = _context.SubCategory.ToList();

            return View(selectCategorys);
        }
    }
}