using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Board.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Board.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        [Authorize(Roles = "Администратор")]
        public ActionResult AllRoles()
        {
            ViewBag.Roles = _context.Roles.ToList();

            return View();
        }

        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> Delete(string Id)
        {
            var selectRole = _context.Roles.FirstOrDefault(a => a.Id == Id);

            if (selectRole != null)
            {
                _context.Roles.Remove(selectRole);
                _context.SaveChanges();
            }

            return RedirectToAction("AllRoles");
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult AddRoleUser()
        {
            var users = _context.Users.ToList();

            if (users != null)
            {
                ViewBag.User = users;
            }

            ViewBag.Roles = _context.Roles.ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult> AddUserRoles(string RoleId, string UserId)
        {
            var selectUser = _context.Users.FirstOrDefault(a => a.Id == UserId);

            var selectRole = _context.Roles.FirstOrDefault(t => t.Id == RoleId);

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            if (selectUser != null && selectRole != null)
            {
                userManager.AddToRole(selectUser.Id, selectRole.Name);

                TempData["Flag"] = "Success";
            }
            else
            {
                TempData["Flag"] = "Fail";
            }

            return RedirectToAction("AddRoleUser");
        }

        [Authorize(Roles = "Администратор")]
        public ActionResult AddRole()
        {
            return View();
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public async Task<ActionResult> CreateNewRole(string NameRole)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            if (NameRole != null || NameRole != string.Empty)
            {
                var role = new IdentityRole { Name = NameRole };

                roleManager.Create(role);

                TempData["Flag"] = "Success";
            }
            else
            {
                TempData["Flag"] = "Fail";
            }

            return RedirectToAction("AddRole");
        }
    }
}