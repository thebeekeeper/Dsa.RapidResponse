using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Dsa.RapidResponse.Services
{
    // store availability ranges as start/end times on a day of the week
    public class Availability
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        public int Id { get; set; }
        public virtual IdentityUser User { get; set; }
        public long StartMinute { get; set; }
        public long EndMinute { get; set; }
        public int DayOfWeek { get; set; }
    }
}