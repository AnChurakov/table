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

        public ActionResult Index()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                var selectCategorys = _context.Categorys.ToList();

                ViewBag.SubCategory = _context.SubCategory.ToList();

                return View(selectCategorys);

            }
            catch(Exception e)
            {
                logger.Error(e);
            }
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}