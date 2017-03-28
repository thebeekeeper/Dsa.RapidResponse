using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace Dsa.RapidResponse.Services
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string ExternalLink { get; set; }
        public string Details { get; set; }
        //public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public Event()
        {
            //ApplicationUsers = new List<ApplicationUser>();
        }
    }
}