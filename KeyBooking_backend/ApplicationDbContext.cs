using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace KeyBooking_backend
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            AddRoles(builder);
        }

        private void AddRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "Deanery", ConcurrencyStamp = "2", NormalizedName = "DEANERY" },
                new IdentityRole() { Name = "Teacher", ConcurrencyStamp = "3", NormalizedName = "TEACHER" },
                new IdentityRole() { Name = "Student", ConcurrencyStamp = "4", NormalizedName = "STUDENT" }
                );
        }
    }
}