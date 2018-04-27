using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Board.Models;

namespace Board.Controllers
{
    public class AdsController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult AddAds()
        {
            ViewBag.City = _context.City.ToList();
            ViewBag.Category = _context.Categorys.ToList();
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewAds(Ads model, Guid City, Guid Category)
        {

            return View("AddAds");
        };

        
    }
}