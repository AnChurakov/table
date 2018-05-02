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

        public ActionResult AllAds(Guid Id, string SubcatId)
        {
            List<Ads> listAds = new List<Ads>();

            var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == Id);

            var subCategoryId = new Guid(SubcatId);

            ViewBag.Category = selectCat.Name;

            if (SubcatId == null)
            {
                listAds = _context.Ads.Where(a => a.Categorys.Special == true && a.Categorys.Id == selectCat.Id)
                        .OrderByDescending(t => t.DateCreation)
                        .ToList();
            }
            else
            {
                listAds = _context.Ads.Where(a => a.SubCategory.Id == subCategoryId && a.Categorys.Id == selectCat.Id)
                        .OrderByDescending(t => t.DateCreation)
                        .ToList();
            }

            ViewBag.Citys = _context.City.OrderBy(a => a.Name).ToList();

            ViewBag.SubCategory = subCategoryId;

            ViewBag.CategoryId = Id;

            return View(listAds);
        }

        [Authorize]
        public ActionResult Edit(Guid Id)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            return View(selectAds);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAds(string NameAds, string DescAds, Guid AdsId)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == AdsId);

            if (NameAds != null || NameAds != string.Empty)
            {
                selectAds.Name = NameAds;
            }

            if (DescAds != null || DescAds != string.Empty)
            {
                selectAds.Description = DescAds;
            }

            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = AdsId});
        }

        [HttpPost]
        public ActionResult SearchSingle (string Keyword, Guid CategoryId, Guid SubCatId)
        {
            var selectAds = _context.Ads.Where(a => a.Name.Contains(Keyword) && a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                 .ToList();

            return PartialView(selectAds);
        }

        [HttpPost]
        public ActionResult SortCityAds(Guid CityId, Guid CategoryId, Guid SubCatId)
        {
            var selectAds = _context.Ads.Where(a => a.Citys.Id == CityId && a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                .ToList();

            return PartialView(selectAds);
        }

        [HttpPost]
        public ActionResult AdsSearch(string nameSearch)
        {
            List<Ads> resultSearch = new List<Ads>();

            string[] splitSearch = nameSearch.Split(' ');

            foreach (var search in splitSearch)

            {
                resultSearch = _context.Ads.Where(a => a.Name.Contains(search)).ToList();
            }

            ViewBag.NameSearch = nameSearch;

            return View(resultSearch);
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
                Description = model.Description,
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

            var selectComplaint = _context.Complaints.Where(a => a.Ads.Id == Id).ToList();

            var selectImages = _context.ImageAds.Where(a => a.Ads.Id == Id).ToList();

            if (selectImages != null)
            {
                foreach(var deleteImages in selectImages)
                {
                    _context.ImageAds.Remove(deleteImages);
                }
            }

            if (selectComplaint != null)
            {
                foreach (var deleteComplaint in selectComplaint)
                {
                    _context.Complaints.Remove(deleteComplaint);
                };
            }

            _context.Ads.Remove(selectAds);

            _context.SaveChanges();

            return RedirectToAction(action, controller);
        }
        
    }
}