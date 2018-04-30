using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Board.Models;
using System.Threading.Tasks;

namespace Board.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        
        public ActionResult AllCategory()
        {
            return View(_context.Categorys.ToList());
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewCat(Category model)
        {
         
           if (ModelState.IsValid)
            {             
                Category cat = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Special = model.Special
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
        public async Task<ActionResult> Delete(Guid Id)
        {
            var selectCat = _context.Categorys.FirstOrDefault(a => a.Id == Id);

            _context.Categorys.Remove(selectCat);
            _context.SaveChanges();

            return RedirectToAction("AllCategory");
        }
    }
}