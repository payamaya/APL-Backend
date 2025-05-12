using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<FileRecord> FileRecords { get; set; }
        public DbSet<Student> Students => Set<Student>();
        public DbSet<EmailVerification> EmailVerifications => Set<EmailVerification>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>(); // This stores the enum as a string

            modelBuilder
                .Entity<Teacher>()
                .Property(t => t.TeacherType)
                .HasConversion<string>();

            modelBuilder
                .Entity<Activity>()
                .Property(a => a.ActivityType)
                .HasConversion<string>();
        }

    }
}
