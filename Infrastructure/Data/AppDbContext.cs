using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    
public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<UserCourse> UserCourses { get; set; } = null!;
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
            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId);

            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId);

            modelBuilder
                .Entity<Activity>()
                .Property(a => a.ActivityType)
                .HasConversion<string>();

            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.UserId);
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.User)
                .WithOne()
                .HasForeignKey<Teacher>(t => t.UserId); // Removed .HasConversion<string>() as it is not valid here

            modelBuilder.Entity<Student>()
                .HasKey(t => t.UserId);
            modelBuilder.Entity<Student>()
                .HasOne(t => t.User)
                .WithOne()
                .HasForeignKey<Student>(t => t.UserId); // Removed .HasConversion<string>() as it is not valid here

            modelBuilder.Entity<Teacher>()
                .Property(t => t.TeacherType)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
