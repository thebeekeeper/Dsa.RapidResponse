using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using Dsa.RapidResponse.Implementations;
using Dsa.RapidResponse.Services;

// this mostly came from http://www.blinkingcaret.com/2016/11/30/asp-net-identity-core-from-scratch/
namespace Dsa.RapidResponse
{
    public class AvailabilityController : Controller
    {
        public AvailabilityController(ComradeDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var u = await _userManager.GetUserAsync(HttpContext.User);
            var a = _context.Availabilities.Where(x => x.User == u);
            var models = (from y in a
                select new AvailabilityModel()
                {
                    DayOfWeek = _days[y.DayOfWeek],
                    Start = DateTime.Today.AddMinutes(y.StartMinute),
                    End = DateTime.Today.AddMinutes(y.EndMinute),
                }).OrderBy(m => m.DayOfWeek);
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
        private readonly UserManager<IdentityUser> _userManager;

        private List<string> _days = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    }

    public class AvailabilityModel
    {
        [Display(Name = "Day of the week")]
        public string DayOfWeek { get; set; }
        // TimeSpan might work better for these, but I don't see a html helper for them
        [Display(Name = "Start Time")]
        //public TimeSpan? Start { get; set; }
        //public string Start { get; set; }
        public DateTime Start { get; set; }
        [Display(Name = "End Time")]
        public DateTime End { get; set; }
        //public TimeSpan End { get; set; }
    }
}