using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Board.Models;
using System.Threading.Tasks;

namespace Board.Controllers
{
    public class CityController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult AllCity()
        {
            return View(_context.City.ToList());
        }

        public ActionResult AddCity()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewCity(City model)
        {
            if (ModelState.IsValid)
            {
                City city = new City
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                _context.City.Add(city);
                _context.SaveChanges();

                ViewBag.Flag = "Success";
            }
            else
            {
                ViewBag.Flag = "Fail";
            }

            return View("AddCity");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var selectCity = _context.City.FirstOrDefault(a => a.Id == Id);

            _context.City.Remove(selectCity);
            _context.SaveChanges();

            return RedirectToAction("AllCity");
        }
    }
}