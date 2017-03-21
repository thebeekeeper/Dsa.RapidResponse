using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dsa.RapidResponse.Services
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }
}