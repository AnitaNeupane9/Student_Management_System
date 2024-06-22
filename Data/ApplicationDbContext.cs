using AnitaSMS.Infrastructure.Entity_Configuration;
using AnitaSMS.Models.Entity;
using AnitaSMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnitaSMS.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Students> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);


            builder.Entity<ApplicationUser>()
                .Property(e => e.MiddleName)
                .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                .Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(100);


            builder.Entity<ApplicationUser>()
                .Property(e => e.ProfileUrl)
                .HasMaxLength(500);


            builder.Entity<ApplicationUser>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.Entity<ApplicationUser>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("GETDATE()");


            builder.Entity<ApplicationUser>()
                .Property(e => e.CreatedBy)
                .HasMaxLength(100);


            builder.Entity<ApplicationUser>()
                .Property(e => e.ModifiedBy)
                .HasMaxLength(100);


            builder.Entity<IdentityRole>()
                .ToTable("Roles")
                .Property(p => p.Id)
                .HasColumnName("RoleId");


            // Apply configurations for other entities
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new StudentConfiguration());
        }
    }
}
