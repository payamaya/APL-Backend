using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Activity = Domain.Entities.Activity;
using Module = Domain.Entities.Module;

namespace Infrastructure.Data
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Module> Modules => Set<Module>();

        public DbSet<Activity> Activities => Set<Activity>();

        public DbSet<User> Users => Set<User>();



        //BUG: Need to be fixed!
        // Optionally, implement OnModelCreating if you need more configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // You can enable the many-to-many relationship if needed
            modelBuilder.Entity<User>()
                .HasMany(u => u.Courses)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserCourses")); // Optional join table name
        }

    }
}
