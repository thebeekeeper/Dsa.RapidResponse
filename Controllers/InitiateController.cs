using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Dsa.RapidResponse.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dsa.RapidResponse
{
    [Authorize(Roles = "Administrator")]
    public class InitiateController : Controller
    {
        public InitiateController(IMessagingService messaging)
        {
            _messaging = messaging;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAction(NewActionModel model)
        {
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("it worked!!!");
                _messaging.SendMessage(model.Message);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private readonly IMessagingService _messaging;
    }

    public class NewActionModel
    {
        public string Message {get;set;}
    }
}
