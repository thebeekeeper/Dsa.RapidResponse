using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Implementations;
using Dsa.RapidResponse.Models;

namespace Dsa.RapidResponse
{
    [Authorize(Roles = "Administrator")]
    public class ManagementController : Controller
    {
        public ManagementController(ComradeDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var u = _db.AppUsers;
            return View(u);
        }

        public IActionResult UserDetails(string id)
        {
            var u = _db.AppUsers.FirstOrDefault(x => x.Id.Equals(id));
            if(u == default(ApplicationUser))
                return StatusCode(500);
            var availability = (from y in _db.Availabilities.Where(a => a.User.Id.Equals(id))
                select new AvailabilityModel()
                {
                    Id = y.Id,
                    DayOfWeek = _days[y.DayOfWeek],
                    Start = DateTime.Today.AddMinutes(Convert.ToDouble(y.StartMinute)),
                    End = DateTime.Today.AddMinutes(Convert.ToDouble(y.EndMinute)),
                }).OrderBy(m => m.DayOfWeek).ToList();
            return View(availability);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var logins = user.Logins;
            var rolesForUser = await _userManager.GetRolesAsync(user);

            using (var transaction = _db.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }

                await _userManager.DeleteAsync(user);
                transaction.Commit();
            }
            return RedirectToAction("Users");
        }

        private ComradeDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private List<string> _days = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    }
}