using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Dsa.RapidResponse.Implementations;

namespace Dsa.RapidResponse.Services
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<EventUser> EventUsers { get; set; }
        public ApplicationUser()
        {
        }
    }
}