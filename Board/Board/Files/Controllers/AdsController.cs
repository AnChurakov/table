using System;
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

        //Для вывода всех объявлений на странице Все объявления
        public ActionResult AllAdsInProject(int? page)
        {
            List<Ads> _selectAds = new List<Ads>();

            int sizePage = 30;

            int pageNumber = (page ?? 1);

            if (Session["SaveCityId"] != null)
            {
                var CityId = Session["SaveCityId"].ToString();

                _selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId))
                    .OrderByDescending(a => a.DateCreation)
                    .ToList();

                Session["CountAds"] = _selectAds.Count;

            }
            else
            {
                _selectAds = _context.Ads.OrderByDescending(t => t.DateCreation).ToList();

                Session["CountAds"] = _selectAds.Count;

            }

            //Вывод города Ижевск отдельного из списка остальных городов
            ViewBag.Citys = _context.City.Where(t => t.Name != "Ижевск").OrderBy(a => a.Name).ToList();

            ViewBag.Izh = _context.City.FirstOrDefault(a => a.Name == "Ижевск");
            
            //Вывод баннеров на странице
            ViewBag.Banner = _context.ImageBanners.Where(a => a.Banners.SinglePage == false)
                .OrderByDescending(t => t.DateCreate)
                .ToList();

            return View(_selectAds.ToPagedList(pageNumber, sizePage));
        }

        //Вывод всех объвлений в выбранной категории
        public ActionResult AllAds(string translitCat, int? page, string subcattrans = null)
        {
            List<Ads> listAds = new List<Ads>();

            int sizePage = 30;

            int pageNumber = (page ?? 1);

            var selectCat = _context.Categorys.FirstOrDefault(a => a.Transliteration == translitCat);

            ViewBag.Category = selectCat.Name;

            if (subcattrans == null)
            {
                if (Session["SaveCityId"] == null)
                {
                    listAds = _context.Ads.Where(a => a.Categorys.Special == true && a.Categorys.Transliteration == selectCat.Transliteration)
                            .OrderByDescending(t => t.DateCreation)
                            .ToList();

                    Session["CountAds"] = listAds.Count;
                }
                else 
                {
                    var CityId = Session["SaveCityId"].ToString();

                    listAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId) && a.Categorys.Special == true && a.Categorys.Transliteration == selectCat.Transliteration)
                            .OrderByDescending(t => t.DateCreation)
                            .ToList();

                    Session["CountAds"] = listAds.Count;

                    ViewBag.SaveCityName = Session["SaveCityName"].ToString();
                }
            }
            else
            {
                if (Session["SaveCityId"] == null)
                {
                    listAds = _context.Ads.Where(a => a.SubCategory.Transliteration == subcattrans && a.Categorys.Transliteration == selectCat.Transliteration)
                            .OrderByDescending(t => t.DateCreation)
                            .ToList();

                    Session["CountAds"] = listAds.Count;
                }
                else
                {
                    var CityId = Session["SaveCityId"].ToString();

                    listAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId) && a.SubCategory.Transliteration == subcattrans && a.Categorys.Transliteration == selectCat.Transliteration)
                                                .OrderByDescending(t => t.DateCreation)
                                                .ToList();

                    //Получение количества объявлений
                    Session["CountAds"] = listAds.Count;

                    ViewBag.SaveCityName = Session["SaveCityName"].ToString();
                }
            }

            ViewBag.Citys = _context.City.Where(t => t.Name != "Ижевск").OrderBy(a => a.Name).ToList();

            ViewBag.Izh = _context.City.FirstOrDefault(a => a.Name == "Ижевск");

            if (subcattrans != null)
            {
                ViewBag.SubCategory = _context.SubCategory.FirstOrDefault(a => a.Transliteration == subcattrans).Id;

                var selectNameSubcat = _context.SubCategory.FirstOrDefault(a => a.Transliteration == subcattrans).Name;

                ViewBag.SubCatName = selectNameSubcat;
            }

            ViewBag.CategoryId = selectCat.Id;

            //Вывод баннеров на странице
            ViewBag.Banner = _context.ImageBanners.Where(a => a.Banners.SinglePage == false)
                .OrderByDescending(t => t.DateCreate)
                .ToList();

            return View(listAds.ToPagedList(pageNumber, sizePage));
        }

        [Authorize]
        public ActionResult Edit(Guid Id)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            return View(selectAds);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteImage(Guid ImageId, Guid AdsId)
        {
            var selectAds = _context.Ads.FirstOrDefault(t => t.Id == AdsId);

            if (ImageId != null)
            {
                var selectImage = _context.ImageAds.FirstOrDefault(a => a.Id == ImageId);

                _context.ImageAds.Remove(selectImage);

                _context.SaveChanges();
            }

            return PartialView(selectAds);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAds(string NameAds, string DescAds, Guid AdsId,
            IEnumerable<HttpPostedFileBase> upload)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == AdsId);
            

            if (NameAds != null || NameAds != string.Empty)
            {
                selectAds.Name = NameAds;

                selectAds.Transliteration = Transliteration.Front(selectAds.Name);

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


            if (upload != null)
            {
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
                            Ads = selectAds,
                            Path = correctPathFile
                        };

                        _context.ImageAds.Add(image);

                    }
                    
                }
            }
            //else
            //{
            //    ImageAds img = new ImageAds
            //    {
            //        Id = Guid.NewGuid(),
            //        Ads = selectAds,
            //        Path = "/Files/no-photo.png"
            //    };

            //    _context.ImageAds.Add(img);

            //}

            _context.SaveChanges();

            return RedirectToAction("SelectAds", new {adstrans = selectAds.Transliteration,
                catTrans = selectAds.Categorys.Transliteration,id = selectAds.Id});
        }

        [HttpPost]
        public ActionResult SearchSingle (string Keyword, Guid? CategoryId, Guid? SubCatId)
        {
            List<Ads> listAds = new List<Ads>();

            List<Ads> select = new List<Ads>();

            string[] arrayKeywords = Keyword.Split(' ', '!', '\'', ',', '.', '-');

            foreach (var singleKeyword in arrayKeywords)
            {
                if (CategoryId != null && SubCatId != null)
                {
                     select = _context.Ads.Where(a => a.Name.Contains(singleKeyword) && a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                        .OrderByDescending(t => t.DateCreation)
                        .ToList();
                }
                else
                {
                     select = _context.Ads.Where(a => a.Name.Contains(singleKeyword))
                        .OrderByDescending(t => t.DateCreation)
                        .ToList();
                }

                foreach(var addAds in select)
                {

                    var checkListAds = listAds.FirstOrDefault(a => a.Name.Contains(addAds.Name));

                    if (checkListAds == null)
                    {
                        listAds.Add(addAds);
                    }
                }
            }

            return PartialView(listAds);
        }

        //Сортировка по городам в категориях
        [HttpPost]
        public ActionResult SortCityAds(string CityId, int? page, Guid? CategoryId, Guid? SubCatId)
        {
            List<Ads> selectAds = new List<Ads>();

            int sizePage = 30;

            int pageNumber = (page ?? 1);

            if (CityId != "AllCity")
            {
                var _selectCity = _context.City.FirstOrDefault(a => a.Id == new Guid(CityId));

                Session["SaveCityName"] = _selectCity.Name;

                Session["SaveCityId"] = _selectCity.Id;
            }

            if (CityId == "AllCity")
            {
    
                if (CategoryId != null && SubCatId != null)
                {
                    Session["SaveCityName"] = "AllCity";

                    Session["SaveCityId"] = null;

                    selectAds = _context.Ads.Where(a => a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                        .OrderByDescending(y => y.DateCreation)
                        .ToList();

                    Session["CountAds"] = selectAds.Count;

                    
                }
                else if (CategoryId != null)
                {
                    Session["SaveCityName"] = "AllCity";

                    Session["SaveCityId"] = null;

                    selectAds = _context.Ads.Where(a => a.Categorys.Id == CategoryId).OrderByDescending(y => y.DateCreation)
                        .ToList();

                    Session["CountAds"] = selectAds.Count;

                   
                }
                else
                {
                    Session["SaveCityName"] = null;

                    Session["SaveCityId"] = null;

                    selectAds = _context.Ads.OrderByDescending(y => y.DateCreation)
                        .ToList();

                    Session["CountAds"] = selectAds.Count;

                }
            }
            else if(CityId != "AllCity" && CategoryId != null && SubCatId != null)
            {
                selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId) && a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                    .OrderByDescending(y => y.DateCreation)
                    .ToList();

               
            }
            else if (CityId != "AllCity" && CategoryId != null)
            {
                selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId) && a.Categorys.Id == CategoryId)
                    .OrderByDescending(t => t.DateCreation)
                    .ToList();

                Session["CountAds"] = selectAds.Count;

                
            }
            else if(CityId != "AllCity" && CategoryId == null)
            {
                selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId)).OrderByDescending(t => t.DateCreation)
                    .ToList();

                Session["CountAds"] = selectAds.Count;

            }

            if (SubCatId != null)
            {
                TempData["SubCat"] = _context.SubCategory.FirstOrDefault(a => a.Id == SubCatId).Transliteration;
            }

            TempData["Category"] = _context.Categorys.FirstOrDefault(a => a.Id == CategoryId).Transliteration;

            return PartialView(selectAds.ToPagedList(pageNumber, sizePage));
        }

        //Сортировка по городам на странице Все объявления
        [HttpPost]
        public ActionResult SortAdsCityAllAds(string CityId, int? page, Guid? CategoryId, Guid? SubCatId)
        {
            List<Ads> selectAds = new List<Ads>();

            int sizePage = 30;

            int pageNumber = (page ?? 1);

            if (CityId != "AllCity")
            {
                var _selectCity = _context.City.FirstOrDefault(a => a.Id == new Guid(CityId));

                Session["SaveCityName"] = _selectCity.Name;

                Session["SaveCityId"] = _selectCity.Id;
            }

            if (CityId == "AllCity")
            {

                if (CategoryId != null && SubCatId != null)
                {
                    Session["SaveCityName"] = "AllCity";

                    Session["SaveCityId"] = null;

                    selectAds = _context.Ads.Where(a => a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                        .OrderByDescending(y => y.DateCreation)
                        .ToList();

                    Session["CountAds"] = selectAds.Count;

                }
                else if (CategoryId != null)
                {
                    Session["SaveCityName"] = "AllCity";

                    Session["SaveCityId"] = null;

                    selectAds = _context.Ads.Where(a => a.Categorys.Id == CategoryId).OrderByDescending(y => y.DateCreation)
                        .ToList();

                    Session["CountAds"] = selectAds.Count;

                }
                else
                {
                    Session["SaveCityName"] = null;

                    Session["SaveCityId"] = null;

                    selectAds = _context.Ads.OrderByDescending(y => y.DateCreation)
                        .ToList();

                    Session["CountAds"] = selectAds.Count;

                }
            }
            else if (CityId != "AllCity" && CategoryId != null && SubCatId != null)
            {
                selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId) && a.Categorys.Id == CategoryId && a.SubCategory.Id == SubCatId)
                    .OrderByDescending(y => y.DateCreation)
                    .ToList();

                Session["CountAds"] = selectAds.Count;

            }
            else if (CityId != "AllCity" && CategoryId != null)
            {
                selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId) && a.Categorys.Id == CategoryId)
                    .OrderByDescending(t => t.DateCreation)
                    .ToList();

                Session["CountAds"] = selectAds.Count;

            }
            else if (CityId != "AllCity" && CategoryId == null)
            {
                selectAds = _context.Ads.Where(a => a.Citys.Id == new Guid(CityId)).OrderByDescending(t => t.DateCreation)
                    .ToList();

                Session["CountAds"] = selectAds.Count;

            }

            return PartialView(selectAds.ToPagedList(pageNumber, sizePage));
        }

        [HttpPost]
        public ActionResult AdsSearch(string nameSearch)
        {

            List<Ads> listAds = new List<Ads>();

            string[] arrayKeywords = nameSearch.Split(' ', '!', '\'', ',', '.');

            foreach (var singleKeyword in arrayKeywords)
            {
                var select = _context.Ads.Where(a => a.Name.Contains(singleKeyword)).ToList();

                foreach (var addAds in select)
                {
                    var checkListAds = listAds.FirstOrDefault(a => a.Name.Contains(addAds.Name));

                    if (checkListAds == null)
                    {
                        listAds.Add(addAds);
                    }
                }
            }

            ViewBag.NameSearch = nameSearch;

            return View(listAds.OrderByDescending(a => a.DateCreation));
        }
        
        public ActionResult SelectAds(Guid? Id, string adstrans, string catTrans, string subcattrans = null)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            ViewBag.ListImage = _context.ImageAds.Where(a => a.Ads.Id == Id).ToList();

            //фотография для социально сети
            //var socialImage = _context.ImageAds.FirstOrDefault(a => a.Ads.Id == Id).Path;

            //if (socialImage != null)
            //{
            //    ViewBag.FirstImageSocial = _context.ImageAds.FirstOrDefault(a => a.Ads.Id == Id).Path;
            //}

            //изображения баннеров
            ViewBag.Banner = _context.ImageBanners.Where(a => a.Banners.SinglePage == true)
                .OrderByDescending(t => t.DateCreate)
                .ToList();

            //Название категории
            ViewBag.Category = selectAds.Categorys.Name;

            if (selectAds.SubCategory != null)
            {
                ViewBag.SubCatName = selectAds.SubCategory.Name;
            }

            return View(selectAds);
        } 


        [Authorize]
        public ActionResult AddAds()
        {
            ViewBag.City = _context.City.Where(t => t.Name != "Ижевск").OrderBy(a => a.Name).ToList();

            ViewBag.Izh = _context.City.FirstOrDefault(a => a.Name == "Ижевск");

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

                    if (upload != null)
                    {
                        foreach (var file in upload)
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
                    /*else
                    {
                        ImageAds img = new ImageAds
                        {
                            Id = Guid.NewGuid(),
                            Ads = ads,
                            Path = "/Files/no-photo.png"
                        };

                        _context.ImageAds.Add(img);

                    }*/

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

            string action = dataPath[4];

            string controller = dataPath[3];

            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            var selectComplaint = _context.Complaints.Where(a => a.Ads.Id == Id).ToList();

            var selectImages = _context.ImageAds.Where(a => a.Ads.Id == Id).ToList();

            if (selectImages != null)
            {
                foreach (var deleteImages in selectImages)
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