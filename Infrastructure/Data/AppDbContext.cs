using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using Activity = Domain.Entities.Activity;
using Module = Domain.Entities.Module;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Teacher> Teachers => Set<Teacher>();

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<ActivityAttachment> ActivityAttachments => Set<ActivityAttachment>(); // DbSet for attachments

        
        // Optionally, implement OnModelCreating if you need more configurations
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var comparer = new ValueComparer<List<string>>(
                (a, b) => (a ?? new()).SequenceEqual(b ?? new()),
                a => (a ?? new()).Aggregate(0, (h, s) => HashCode.Combine(h, s.GetHashCode())),
                a => a == null ? new() : new(a)
            );

            builder.Entity<Activity>()
                .Property(a => a.AttachmentUrls)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                )
                .Metadata
                .SetValueComparer(comparer);

            // You can enable the many-to-many relationship if needed
            builder.Entity<User>()
                .HasMany(u => u.Courses)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserCourses")); // Optional join table name

            builder
                .Entity<Teacher>()
                .Property(t => t.TeacherType)
                .HasConversion<string>();

            builder
                .Entity<Activity>()
                .Property(a => a.ActivityType)
                .HasConversion<string>();
        }

    }
}
