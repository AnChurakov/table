using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Board.Models;

namespace Board.Controllers
{
    public class ComplaintController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [Authorize]
        public ActionResult AllComplaint()
        {
            var listComplaint = _context.Complaints.ToList();

            return View(listComplaint);
        }

        [Authorize]
        [HttpGet]
        public RedirectToRouteResult AddComplaint(Guid Id)
        {
            var selectAds = _context.Ads.FirstOrDefault(a => a.Id == Id);

            var selectUser = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name);

            if (selectAds != null && selectUser != null)
            {
                Complaint com = new Complaint
                {
                    Id = Guid.NewGuid(),
                    Ads = selectAds,
                    User = selectUser
                };

                _context.Complaints.Add(com);
                _context.SaveChanges();
                
            }
            
            return RedirectToAction("SelectAds", "Ads", new { id = Id });
        }
    }
}