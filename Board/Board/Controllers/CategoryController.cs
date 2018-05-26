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
                var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == model.Id);

                selectCat.Name = model.Name;

                _context.SaveChanges();
            }

            return RedirectToAction("AllCategory");
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

                ViewBag.Flag = "Success";
            }
           else
            {
                ViewBag.Flag = "Fail";
            }

            return RedirectToAction("AddCategory");
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == Id);

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
                var selectCategory = _context.Categorys.FirstOrDefault(a => a.Id == new Guid(CategoryId));

                if (selectCategory.Special == true)
                {
                    flag = true;
                }
            }      

            return flag;
        }
    }
}