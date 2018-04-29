using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Board.Models;
using System.Web.Routing;

namespace Board.Controllers
{
    public class AdsController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        
        [Authorize]
        public ActionResult MyAds()
        {
            var selectAds = _context.Ads.Where(a => a.User.UserName == User.Identity.Name).ToList();

            return View(selectAds);
        }

        [Authorize]
        public ActionResult GetAllAds()
        {
            return View(_context.Ads.ToList());
        }
        
        public ActionResult AllAds(string name, Guid Id)
        {
            var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == Id);

            ViewBag.Category = selectCat.Name;

            if (name == "my")
            {
                ViewBag.Ads = _context.Ads.Where(a => a.SubCategory.Name == "Нашел" && a.Categorys.Id == selectCat.Id)
                    .OrderByDescending(t => t.DateCreation)
                    .ToList();

                ViewBag.Name = name;

                ViewBag.CategoryId = Id;
                
            }

            if (name == "search")
            {
                ViewBag.Ads = _context.Ads.Where(a => a.SubCategory.Name == "Ищу" && a.Categorys.Id == selectCat.Id)
                    .ToList();

                ViewBag.Name = name;

                ViewBag.CategoryId = Id;
            }

        
            return View();
        }

        [Authorize]
        public ActionResult Edit(Guid Id)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            return View(selectAds);
        }

        [HttpPost]
        public ActionResult AdsSearch(string nameSearch)
        {
            var search = _context.Ads.Where(a => a.Name.Contains(nameSearch)).ToList();

            ViewBag.NameSearch = nameSearch;

            return View(search);
        }
        
        public ActionResult SelectAds(Guid Id)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            ViewBag.ListImage = _context.ImageAds.Where(a => a.Ads.Id == Id).ToList();

            return View(selectAds);
        }


        [Authorize]
        public ActionResult AddAds()
        {
            ViewBag.City = _context.City.ToList();
            ViewBag.Category = _context.Categorys.ToList();
            ViewBag.SubCat = _context.SubCategory.ToList();
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewAds(Ads model, Guid City, Guid Category, Guid SubCat,
            HttpPostedFileBase upload)
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

            string fileName = System.IO.Path.GetFileName(upload.FileName);

            var pathFile = System.IO.Path.Combine(Server.MapPath("~/Files"), fileName);

            upload.SaveAs(pathFile);

            var splitPathFile = pathFile.Split('\\');

            string correctPathFile = string.Format("/{0}/{1}", splitPathFile[5], splitPathFile[6]);

            ImageAds image = new ImageAds
                {
                    Id = Guid.NewGuid(),
                    Ads = ads,
                    Path = correctPathFile
                };            

            _context.ImageAds.Add(image);
            _context.Ads.Add(ads);

            _context.SaveChanges();

            return RedirectToAction("SelectAds", new { id = ads.Id});
        }

        [HttpGet]
        public RedirectToRouteResult Delete(Guid Id, string url)
        {
            string[] dataPath = url.Split('/');

            string controller = dataPath[3];

            string action = dataPath[4];

            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            var selectComplaint = _context.Complaints.FirstOrDefault(a => a.Ads.Id == Id);

            if (selectComplaint != null)
            {
                _context.Complaints.Remove(selectComplaint);
            }

            _context.Ads.Remove(selectAds);

            _context.SaveChanges();

            return RedirectToAction(action, controller);
        }
        
    }
}