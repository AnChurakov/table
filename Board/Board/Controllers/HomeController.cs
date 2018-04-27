using System;
using System.Collections.Generic;
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
            return View(_context.Categorys.ToList());
        }
    }
}