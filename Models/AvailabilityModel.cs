using System;
using System.ComponentModel.DataAnnotations;

namespace  Dsa.RapidResponse.Models
{
    public class AvailabilityModel
    {
        public int Id { get; set; }
        [Display(Name = "Day of the week")]
        public string DayOfWeek { get; set; }
        // TimeSpan might work better for these, but I don't see a html helper for them
        [Display(Name = "Start Time")]
        //public TimeSpan? Start { get; set; }
        //public string Start { get; set; }
        public DateTime Start { get; set; }
        [Display(Name = "End Time")]
        public DateTime End { get; set; }
        //public TimeSpan End { get; set; }
    }
}