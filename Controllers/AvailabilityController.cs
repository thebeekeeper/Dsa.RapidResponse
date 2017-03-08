using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var u = await _userManager.GetUserAsync(HttpContext.User);
            var a = _context.Availabilities.Where(x => x.User == u);
            var models = from y in a
                select new AvailabilityModel()
                {
                    DayOfWeek = y.DayOfWeek,
                    Start = TimeSpan.FromMinutes(y.StartMinute),
                    End = TimeSpan.FromMinutes(y.EndMinute),
                };
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
                    DayOfWeek = newItem.DayOfWeek,
                    StartMinute = (long)newItem.Start.TotalMinutes,
                    EndMinute = (long)newItem.End.TotalMinutes,
                };
                _context.Add(entity);
                _context.SaveChanges();
            }
            return Redirect("List");
        }

        private readonly ComradeDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
    }

    public class AvailabilityModel
    {
        public int DayOfWeek { get; set; }
        // TimeSpan might work better for these, but I don't see a html helper for them
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}