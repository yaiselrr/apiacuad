using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDBContext : IdentityDbContext<GeneralDataUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserProperty>().HasKey(u => new { u.GeneralDataUserId, u.PropertyId });
            builder.Entity<Property>().HasKey(p => new { p.PropertyNumber, p.GeneralDataUserId });
            base.OnModelCreating(builder);
        }

        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<UserProperty> UsersProperties { get; set; }

        
    }
}
