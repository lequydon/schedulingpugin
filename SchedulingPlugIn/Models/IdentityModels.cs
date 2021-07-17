using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchedulingPlugIn.Models.Entity;

namespace SchedulingPlugIn.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(bool proxy = true)
           : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public System.Data.Entity.DbSet<PageRequest> PageRequestDbSet { get; set; }
        public System.Data.Entity.DbSet<PageQuestion> PageQuestionsDbSet { get; set; }
        public System.Data.Entity.DbSet<Customer> CustomerDbSet { get; set; }
        public System.Data.Entity.DbSet<Calendar> CalendarDbSet { get; set; }
        public System.Data.Entity.DbSet<TimeSlot> TimeSlotDbSet { get; set; }
        public System.Data.Entity.DbSet<SessionTimeSlot> SessionTimeSlotDbSet { get; set; }
        public System.Data.Entity.DbSet<CalendarPooling> CalendarPoolingDbSet { get; set; }
    }
}