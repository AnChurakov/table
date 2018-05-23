using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Threading.Tasks;
using Board.Models;

namespace Board.Controllers
{
    public class BannerController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [Authorize(Roles = "Администратор")]
        public Banner GetBanner(Guid Id)
        {
            var selectBanner = _context.Banners.FirstOrDefault(a => a.Id == Id);

            return selectBanner;
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult AllBanners()
        {
            var selectBanners = _context.Banners.ToList();

            return View(selectBanners);
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var selectBanner = GetBanner(Id);

            var selectImgBanner = _context.ImageBanners.FirstOrDefault(t => t.Banners.Id == Id);

            if (selectImgBanner != null)
            {
                _context.ImageBanners.Remove(selectImgBanner);
            }

            _context.Banners.Remove(selectBanner);

            _context.SaveChanges();

            return RedirectToAction("AllBanners");
        }


        [Authorize(Roles = "Администратор")]
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> AddBanner(Banner model, HttpPostedFileBase upload, string SinglePage)
        {
                Banner banner = new Banner
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    SinglePage = CheckSinglePageBanner(SinglePage),
                    Description = model.Description
                };

                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);

                    var pathFile = System.IO.Path.Combine(Server.MapPath("~/Files"), fileName);

                    upload.SaveAs(pathFile);

                    var splitPathFile = pathFile.Split('\\');

                    string correctPathFile = string.Format("/{0}/{1}", splitPathFile[5], splitPathFile[6]);

                    ImageBanner imgban = new ImageBanner
                    {
                        Id = Guid.NewGuid(),
                        Banners = banner,
                        Path = correctPathFile
                    };

                    _context.ImageBanners.Add(imgban);

                }

                _context.Banners.Add(banner);

                _context.SaveChanges();
           

            return RedirectToAction("Index");
        }

        public bool CheckSinglePageBanner(string SinglePage)
        {
            bool flag = false;

            if (SinglePage == "on")
            {
                flag = true;
            }

            return flag;
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult Edit(Guid Id)
        {
            return View(GetBanner(Id));
        }

        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> Update(Banner model, HttpPostedFileBase upload, string SinglePage)
        {
           
                var banner = GetBanner(model.Id);

                banner.Name = model.Name;

                banner.Description = model.Description;

                banner.SinglePage = CheckSinglePageBanner(SinglePage);

                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);

                    var pathFile = System.IO.Path.Combine(Server.MapPath("~/Files"), fileName);

                    upload.SaveAs(pathFile);

                    var splitPathFile = pathFile.Split('\\');

                    string correctPathFile = string.Format("/{0}/{1}", splitPathFile[5], splitPathFile[6]);

                    var selectImg = banner.ImageBanner.FirstOrDefault(a => a.Banners.Name == banner.Name);

                    selectImg.Path = correctPathFile;

                }
            

            _context.SaveChanges();

            return RedirectToAction("AllBanners");
        }


    }
}