
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Implementations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dsa.RapidResponse
{
    [Authorize(Roles = "Administrator")]
    public class ManagementController : Controller
    {
        public ManagementController(ComradeDbContext db, UserManager<IdentityUser> userManager)
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
            return View();
        }

        public IActionResult Events()
        {
            var events = _db.Events;
            return View(events);
        }

        // GET
        public IActionResult AddEvent()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEvent(Event evt)
        {
            return Redirect("/");
        }

        private ComradeDbContext _db;
        private UserManager<IdentityUser> _userManager;
    }
}