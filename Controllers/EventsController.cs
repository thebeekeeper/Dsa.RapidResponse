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
    public class EventsController : Controller
    {
        public EventsController(ComradeDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var events = _db.Events.Where(e => e.Time > DateTime.Now);
            return View(events);
        }

        public IActionResult Details(int id)
        {
            var entity = _db.Events.FirstOrDefault(e => e.Id == id);
            if(entity != default(Event))
            {
                return View(entity);
            }
            return Redirect("/");
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

        private ComradeDbContext _db;

    }
}