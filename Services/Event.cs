using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Dsa.RapidResponse.Implementations;

namespace Dsa.RapidResponse.Services
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string ExternalLink { get; set; }
        public string Details { get; set; }

        public virtual ICollection<EventUser> EventUsers { get; set; }

        public Event()
        {
            EventUsers = new List<EventUser>();
        }
    }
}