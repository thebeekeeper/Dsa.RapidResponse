using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Implementations;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dsa.RapidResponse
{
    [Authorize(Roles = "Administrator")]
    public class InitiateController : Controller
    {
        public InitiateController(IMessagingService messaging, ComradeDbContext db)
        {
            _messaging = messaging;
            _db = db;
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
            // this should create a comfirmation page before sending the messages
            if (ModelState.IsValid)
            {
                var now = DateTime.Now.TimeOfDay;
                var day = (int)DateTime.Now.DayOfWeek;
                // figure out who is available right now
                var available = (from a in _db.Availabilities
                    where a.DayOfWeek == day
                    where a.StartMinute < now.TotalMinutes
                    where a.EndMinute > now.TotalMinutes
                    //where string.IsNullOrEmpty(a.User.Id) == false
                    select a.User).Distinct().ToList();

                // and send them a message
                foreach (var a in available)
                {
                    if(string.IsNullOrEmpty(a.PhoneNumber) == false)
                    {
                        _messaging.SendMessage(a.PhoneNumber, model.Message);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private readonly IMessagingService _messaging;
        private readonly ComradeDbContext _db;
    }

    public class NewActionModel
    {
        public string Message {get;set;}
    }
}
