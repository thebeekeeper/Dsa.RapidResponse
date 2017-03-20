using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dsa.RapidResponse.Implementations;

namespace Dsa.RapidResponse.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ComradeDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var upcoming = _db.Events.Where(e => (e.Time - DateTime.Now).Days < 10);
            return View(upcoming);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private ComradeDbContext _db;
    }
}
