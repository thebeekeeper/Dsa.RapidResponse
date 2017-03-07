using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dsa.RapidResponse
{
    public class SetupController : Controller
    {
        // GET: /<controller>/
        [Authorize(Roles="Administrator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
