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

        [Authorize]
        public ActionResult AllAds(string name, Guid Id)
        {
            var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == Id);

            ViewBag.Category = selectCat.Name;

            if (name == "my")
            {
                ViewBag.Ads = _context.Ads.Where(a => a.SubCategory.Name == "Нашел" && a.Categorys.Id == selectCat.Id).ToList();
            }

            if (name == "search")
            {
                ViewBag.Ads = _context.Ads.Where(a => a.SubCategory.Name == "Ищу" && a.Categorys.Id == selectCat.Id).ToList();
            }

        
            return View();
        }

        [Authorize]
        public ActionResult SelectAds(Guid Id)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            return View(selectAds);
        }

        public ActionResult AddAds()
        {
            ViewBag.City = _context.City.ToList();
            ViewBag.Category = _context.Categorys.ToList();
            ViewBag.SubCat = _context.SubCategory.ToList();
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewAds(Ads model, Guid City, Guid Category, Guid SubCat)
        {
            
                var selectCategory = _context.Categorys.FirstOrDefault(a => a.Id == Category);
                var selectCity = _context.City.FirstOrDefault(a => a.Id == City);
                var selectSubCat = _context.SubCategory.FirstOrDefault(a => a.Id == SubCat);
                var selectUser = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name);

                Ads ads = new Ads
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    DateCreation = DateTime.Now,
                    Categorys = selectCategory,
                    SubCategory = selectSubCat,
                    Citys = selectCity,
                    User = selectUser,
                };

                _context.Ads.Add(ads);
                _context.SaveChanges();
            


            return RedirectToAction("SelectAds", new { id = ads.Id});
        }

        
    }
}