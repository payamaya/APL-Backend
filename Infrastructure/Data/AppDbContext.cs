using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<ActivityAttachment> ActivityAttachments => Set<ActivityAttachment>(); // DbSet for attachments

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    // Configure ActivityAttachment entity
        //    builder.Entity<ActivityAttachment>()             
        //        .ToTable("ActivityAttachments")              // (optional)
        //        .HasKey(a => a.Id);                           

        //    builder.Entity<ActivityAttachment>()            
        //        .Property(a => a.Data)                       
        //        .HasColumnType("bytea");

        //    // Configure one-to-many: Activity → Attachments

        //    builder.Entity<Activity>()                      
        //        .HasMany(a => a.Attachments)                
        //        .WithOne(att => att.Activity)                
        //        .HasForeignKey(att => att.ActivityId);       

        //    // 1) Define a ValueComparer so EF can diff two List<string> values
        //    var listComparer = new ValueComparer<List<string>>(
        //        // Compare: sequence equality
        //        (a, b) => (a ?? new List<string>()).SequenceEqual(b ?? new List<string>()),

        //        // Hash: combine hashes of each element
        //        a => (a ?? new List<string>()).Aggregate(0, (hash, str) => HashCode.Combine(hash, str.GetHashCode())),

        //        // Snapshot: make a deep copy for change‐tracking
        //        a => a == null ? new List<string>() : new List<string>(a)
        //    );

        //    //// 2) Teach EF how to store List<string> as JSON + use our comparer
        //    //builder.Entity<Activity>()
        //    //    .Property(a => a.AttachmentUrls)
        //    //    .HasConversion(
        //    //        urls => JsonSerializer.Serialize(urls, (JsonSerializerOptions?)null),
        //    //        json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null)
        //    //                 ?? new List<string>()
        //    //    )
        //    //    // Attach the comparer
        //    //    .Metadata
        //    //    .SetValueComparer(listComparer);
        //}

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
        }

    }
}
