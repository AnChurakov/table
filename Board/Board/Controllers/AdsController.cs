﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Board.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Routing;
using PagedList.Mvc;
using PagedList;
using Board.Common;

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

        [Authorize(Roles = "Администратор")]
        public ActionResult GetAllAds(int? page)
        {
            int sizePage = 15;

            int pageNumber = (page ?? 1);
            
            var selectAds = _context.Ads.ToList();

            return View(selectAds.ToPagedList(pageNumber, sizePage));
        }

        public ActionResult AllAds(string translitCat, int? page, string subcattrans = null)
        {
            List<Ads> listAds = new List<Ads>();

            int sizePage = 40;

            int pageNumber = (page ?? 1);

            var selectCat = _context.Categorys.FirstOrDefault(a => a.Transliteration == translitCat);

            ViewBag.Category = selectCat.Name;

            if (subcattrans == null)
            {
                listAds = _context.Ads.Where(a => a.Categorys.Special == true && a.Categorys.Transliteration == selectCat.Transliteration)
                        .OrderByDescending(t => t.DateCreation)
                        .ToList();
            }
            else
            {
                listAds = _context.Ads.Where(a => a.SubCategory.Transliteration == subcattrans && a.Categorys.Transliteration == selectCat.Transliteration)
                        .OrderByDescending(t => t.DateCreation)
                        .ToList();
            }

            ViewBag.Citys = _context.City.OrderBy(a => a.Name).ToList();

            if (subcattrans != null)
            {
                ViewBag.SubCategory = _context.SubCategory.FirstOrDefault(a => a.Transliteration == subcattrans).Id;

                var selectNameSubcat = _context.SubCategory.FirstOrDefault(a => a.Transliteration == subcattrans).Name;

                ViewBag.SubCatName = selectNameSubcat;
            }

            ViewBag.CategoryId = selectCat.Id;

            ViewBag.Banner = _context.ImageBanners.Where(a => a.Banners.SinglePage == false).ToList();

            return View(listAds.ToPagedList(pageNumber, sizePage));
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

                TempData["Flag"] = "Success";
            }
            else
            {
                TempData["Flag"] = "Fail";
            }

            if (DescAds != null || DescAds != string.Empty)
            {
                selectAds.Description = DescAds;

                TempData["Flag"] = "Success";
            }
            else
            {
                TempData["Flag"] = "Fail";
            }

            _context.SaveChanges();

            return RedirectToAction("Edit", new { id = selectAds.Id});
        }

        [HttpPost]
        public ActionResult SearchSingle (string Keyword, Guid CategoryId, Guid? SubCatId)
        {
            var selectAds = _context.Ads.Where(a => a.Name.Contains(Keyword) && a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                 .ToList();

            return PartialView(selectAds);
        }

        [HttpPost]
        public ActionResult SortCityAds(string CityId, Guid CategoryId, Guid? SubCatId)
        {
            List<Ads> selectAds = new List<Ads>();

            if (CityId != "AllCity")
            {
                 selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId) && a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                    .ToList();
            }
            else
            {
                selectAds = _context.Ads.Where(a => a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                    .ToList();
            }
            return PartialView(selectAds);
        }

        [HttpPost]
        public ActionResult AdsSearch(string nameSearch)
        {
            List<Ads> resultSearch = new List<Ads>();

            var searchResult = _context.Ads.Where(a => a.Name.Contains(nameSearch)).ToList();

            ViewBag.NameSearch = nameSearch;

            return View(searchResult);
        }
        
        public ActionResult SelectAds(Guid? Id, string adstrans, string catTrans, string subcattrans = null)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            ViewBag.CheckImg = selectAds.ImageAds.Count;

            ViewBag.ListImage = _context.ImageAds.Where(a => a.Ads.Id == Id).ToList();

            ViewBag.Banner = _context.ImageBanners.Where(a => a.Banners.SinglePage == true).ToList();

            return View(selectAds);
        } 


        [Authorize]
        public ActionResult AddAds()
        {
            ViewBag.City = _context.City.OrderBy(a => a.Name).ToList();

            ViewBag.Category = _context.Categorys.OrderBy(a => a.Name).ToList();

            ViewBag.SubCat = _context.SubCategory.OrderBy(a => a.Name).ToList();

            ViewBag.CheckPhoneUser = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).PhoneNumber;

            ViewBag.FirstName = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).FirstName;

            ViewBag.LastName = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).LastName;

            return View();
        }

        [HttpPost]
        public RedirectToRouteResult CreateNewAds(Ads model, Guid City, Guid Category, Guid? SubCat,
            IEnumerable<HttpPostedFileBase> upload)
        {

            var selectCategory = _context.Categorys.FirstOrDefault(a => a.Id == Category);
            var selectCity = _context.City.FirstOrDefault(a => a.Id == City);
            var selectSubCat = _context.SubCategory.FirstOrDefault(a => a.Id == SubCat);
            var selectUser = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name);

            if (selectCategory != null && selectCity != null && selectUser != null)
            {
                var translit = Transliteration.Front(model.Name);

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
                    Transliteration = translit
                };

                foreach (var file in upload)
                {
                    if (file != null)
                    {
                        string fileName = System.IO.Path.GetFileName(file.FileName);

                        var pathFile = System.IO.Path.Combine(Server.MapPath("~/Files"), fileName);

                        file.SaveAs(pathFile);

                        var splitPathFile = pathFile.Split('\\');

                        string correctPathFile = string.Format("/{0}/{1}", splitPathFile[5], splitPathFile[6]);

                        ImageAds image = new ImageAds
                        {
                            Id = Guid.NewGuid(),
                            Ads = ads,
                            Path = correctPathFile
                        };

                        _context.ImageAds.Add(image);

                    }

                }

                _context.Ads.Add(ads);

                _context.SaveChanges();

                ViewBag.Flag = "Success";
            }
            else
            {
                ViewBag.Flag = "Fail";
            }

            return RedirectToAction("MyAds");
        }

        [HttpGet]
        public RedirectToRouteResult Delete(Guid Id, string url)
        {
            string[] dataPath = url.Split('/');

            string action = dataPath[3];

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

            return RedirectToAction("MyAds", "Ads");
        }
        
    }
}