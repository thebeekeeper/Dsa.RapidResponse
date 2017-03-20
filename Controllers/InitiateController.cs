using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Dsa.RapidResponse.Services;
using Dsa.RapidResponse.Implementations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
            var availableUsers = GetAvailableUsers();

            var m = from a in availableUsers select new ConfirmActionModel() { UserEmail = a.Email, UserPhone = a.PhoneNumber };
            return View(m);
        }

        // GET
        public IActionResult CreateAction()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAction(NewActionModel model)
        {
            var availableUsers = GetAvailableUsers();
            // this should create a comfirmation page before sending the messages
            if (ModelState.IsValid && availableUsers != null)
            {
                foreach (var a in availableUsers)
                {
                    if (string.IsNullOrEmpty(a.PhoneNumber) == false)
                    {
                        _messaging.SendMessage(a.PhoneNumber, model.Message);
                    }
                }
            }
            return RedirectToAction("Progress");
        }

        public IActionResult Progress()
        {
            return Content("working on it");
        }

        private IList<IdentityUser> GetAvailableUsers()
        {
            var now = DateTime.Now.TimeOfDay;
            var day = (int)DateTime.Now.DayOfWeek;
            // figure out who is available right now
            var availableUsers = (from a in _db.Availabilities
                               where a.DayOfWeek == day
                               where a.StartMinute < now.TotalMinutes
                               where a.EndMinute > now.TotalMinutes
                               select a.User).Distinct().ToList();
            return availableUsers;
        }

        private readonly IMessagingService _messaging;
        private readonly ComradeDbContext _db;
    }

    public class NewActionModel
    {
        public string Message { get; set; }
    }

    public class ConfirmActionModel
    {
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
    }
}
