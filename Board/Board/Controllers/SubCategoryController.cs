﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Board.Models;
using Board.Common;

namespace Board.Controllers
{
    public class SubCategoryController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [Authorize(Roles = "Администратор")]
        public ActionResult AllSubCategory()
        {
            return View(_context.SubCategory.ToList());
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult AddSubCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewSubCat(SubCategory model)
        {
            if (ModelState.IsValid)
            {
                var translit = Transliteration.Front(model.Name);

                SubCategory sub = new SubCategory
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Transliteration = translit
                };

                _context.SubCategory.Add(sub);
                _context.SaveChanges();

                ViewBag.Flag = "Success";
            }
            else
            {
                ViewBag.Flag = "Fail";
            }

            return View("AddSubCategory");
        }
    }
}