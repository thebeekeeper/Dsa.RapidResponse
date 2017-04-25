using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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
    public class EventsController : Controller
    {
        public EventsController(ComradeDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var events = _db.Events.Where(e => e.Time > DateTime.Now);
            return View(events);
        }

        public async Task<IActionResult> Details(int id)
        {
            var entity = _db.Events.Where(e => e.Id == id).Include(e => e.EventUsers).ThenInclude(x => x.User).First();
            var users = entity.EventUsers.Select(x => x.User);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var model = new Models.EventDetailsModel()
            {
                EventId = entity.Id,
                Title = entity.Title,
                Time = entity.Time,
                ExternalLink = entity.ExternalLink,
                Details = entity.Details,
                UserCount = users.Count(),
                CanRsvp = users.Any(x => x.Id.Equals(currentUser.Id)) == false,
                Users = from u in users
                    select new Models.UserModel()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber
                    }
            };
            return View(model);
        }

        // GET
        [Authorize(Roles = "Administrator")]
        public IActionResult AddEvent()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddEvent(Event evt)
        {
            if(ModelState.IsValid)
            {
                _db.Events.Add(evt);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id)
        {
            var evt = _db.Events.FirstOrDefault(e => e.Id == id);
            return View(evt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Event evt)
        {
            _db.Events.Update(evt);
            _db.SaveChanges();
            return RedirectToAction("Details", new { id = evt.Id });
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteEvent(int id)
        {
            var entity = _db.Events.FirstOrDefault(e => e.Id == id);
            if(entity != default(Event))
            {
                _db.Remove(entity);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SignUp(int id)
        {
            var evt = _db.Events.FirstOrDefault(e => e.Id == id);
            var u = await _userManager.GetUserAsync(HttpContext.User);
            evt.EventUsers.Add(new EventUser()
            {
                User = u,
                Event = evt,
            });
            _db.SaveChanges();
            
            return RedirectToAction("Details", new { id = id });
        }

        private ComradeDbContext _db;
        private UserManager<ApplicationUser> _userManager;
    }
}