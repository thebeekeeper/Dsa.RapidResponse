using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Dsa.RapidResponse.Implementations;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Models;

// this mostly came from http://www.blinkingcaret.com/2016/11/30/asp-net-identity-core-from-scratch/
namespace Dsa.RapidResponse
{
    public class AvailabilityController : Controller
    {
        public AvailabilityController(ComradeDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            var entity = _context.Availabilities.FirstOrDefault(a => a.Id == id);
            if(entity != default(Availability))
            {
                _context.Availabilities.Remove(entity);
                _context.SaveChanges();
            }
            // There was something weird here about it redirecting to this method infinitely
            return Redirect("/Availability");
        }

        public async Task<IActionResult> Index()
        {
            var u = await _userManager.GetUserAsync(HttpContext.User);
            var a = _context.Availabilities.Where(x => x.User == u);
            var models = (from y in a
                select new AvailabilityModel()
                {
                    Id = y.Id,
                    DayOfWeek = _days[y.DayOfWeek],
                    Start = DateTime.Today.AddMinutes(Convert.ToDouble(y.StartMinute)),
                    End = DateTime.Today.AddMinutes(Convert.ToDouble(y.EndMinute)),
                }).OrderBy(m => m.DayOfWeek).ToList();
            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(AvailabilityModel newItem)
        {
            if (ModelState.IsValid)
            {
                var u = await _userManager.GetUserAsync(HttpContext.User);
                var entity = new Availability()
                {
                    User = u,
                    // this is coming as an int for some reason instead of the day name
                    // that's because that's what the value is in the tag...
                    DayOfWeek = Int32.Parse(newItem.DayOfWeek),
                    StartMinute = (long)newItem.Start.TimeOfDay.TotalMinutes,
                    EndMinute = (long)newItem.End.TimeOfDay.TotalMinutes,
                };
                _context.Add(entity);
                _context.SaveChanges();
            }
            return Redirect("Index");
        }

        private readonly ComradeDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private List<string> _days = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    }

}