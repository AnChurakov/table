using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Board.Models;
using System.Threading.Tasks;
using Board.Common;

namespace Board.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [Authorize(Roles = "Администратор")]
        public ActionResult AllCategory()
        {
            return View(_context.Categorys.ToList());
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult Edit(Guid Id)
        {
            var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == Id);

            return View(selectCat);
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public async Task<ActionResult> Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                var selectCat = _context.Categorys.FirstOrDefault(a => a.Name == model.Name);

                if (selectCat.Name != null && selectCat.Name != model.Name)
                {
                    selectCat.Name = model.Name;

                    selectCat.Transliteration = Transliteration.Front(selectCat.Name);

                    _context.SaveChanges();

                    TempData["Flag"] = "Success";
                }
                else
                {
                    TempData["Danger"] = "Danger";
                }
            }
            else
            {
                TempData["Flag"] = "Fail";
            }

            return RedirectToAction("Edit", new { id = model.Id});
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> CreateNewCat(Category model)
        {
           if (ModelState.IsValid)
            {
                var selectCategory = _context.Categorys.FirstOrDefault(a => a.Name == model.Name);

                if (selectCategory == null)
                {
                    var translit = Transliteration.Front(model.Name);

                    Category cat = new Category
                    {
                        Id = Guid.NewGuid(),
                        DateCreate = DateTime.Now,
                        Name = model.Name,
                        Special = model.Special,
                        Transliteration = translit
                    };

                    _context.Categorys.Add(cat);

                    _context.SaveChanges();

                    TempData["Flag"] = "Success";
                }
                else
                {
                    TempData["Danger"] = "Danger";
                }
            }
           else
            {
                TempData["Flag"] = "Fail";
            }

            return RedirectToAction("AddCategory");
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == Id);

            var selectAllAds = _context.Ads.Where(a => a.Categorys.Id == Id).ToList();
            
            foreach(var delete in selectAllAds)
            {
                var selectImg = _context.ImageAds.Where(t => t.Ads.Id == delete.Id).ToList();

                var selectComplaint = _context.Complaints.Where(y => y.Ads.Id == delete.Id).ToList();

                foreach (var deleteImg in selectImg)
                {
                    _context.ImageAds.Remove(deleteImg);
                }

                foreach(var deleteComplaint in selectComplaint)
                {
                    _context.Complaints.Remove(deleteComplaint);
                }

                _context.Ads.Remove(delete);
            }

            _context.Categorys.Remove(selectCat);

            _context.SaveChanges();

            return RedirectToAction("AllCategory");
        }

        [HttpPost]
        public bool CheckSpecialCategory(string CategoryId)
        {
            bool flag = false;

            if (CategoryId != null || CategoryId != string.Empty)
            {
                var selectCategory = SelectCategory(CategoryId);

                if (selectCategory.Special == true)
                {
                    flag = true;
                }
            }      

            return flag;
        }

       

        public Category SelectCategory(string Id)
        {
            var select = _context.Categorys.FirstOrDefault(a => a.Id == new Guid(Id));

            return select;
        }
    }
}