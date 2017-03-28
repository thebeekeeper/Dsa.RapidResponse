
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
            return View();
        }

        private ComradeDbContext _db;
        private UserManager<ApplicationUser> _userManager;
    }
}