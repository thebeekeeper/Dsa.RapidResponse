using System;
using System.Collections.Generic;

namespace Dsa.RapidResponse.Models
{
    public class EventDetailsModel
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string ExternalLink { get; set; }
        public string Details { get; set; }
        public bool CanRsvp { get; set; }
        public int UserCount { get; set; }
        public IEnumerable<UserModel> Users { get; set; }
    }
}