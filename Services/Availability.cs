using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dsa.RapidResponse.Services
{
    // store availability ranges as start/end times on a day of the week
    public class Availability
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public long StartMinute { get; set; }
        public long EndMinute { get; set; }
        public int DayOfWeek { get; set; }
    }
}