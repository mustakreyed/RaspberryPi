using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RaspberryPIWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public virtual List<Pi> Pis  { get; set; }
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
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Pi> Pis { get; set; }      
        public DbSet<PiHardware> PiHardwares { get; set; }
        public DbSet<PiModel> PiModels { get; set; }
        public DbSet<PiPinforModel> PiPinforModels { get; set; }
        public DbSet<PiPin> PiPins { get; set; }
        public DbSet<Sensors> Sensors { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
    }
}