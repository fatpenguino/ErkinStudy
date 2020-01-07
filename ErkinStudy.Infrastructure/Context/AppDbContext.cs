using System.Security.Cryptography.X509Certificates;
using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long> {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserLesson> UserLessons { get; set; }
        public DbSet<UserOnlineCourse> UserOnlineCourses { get; set; }
        public DbSet<OnlineCourse> OnlineCourses { get; set; }
        public DbSet<OnlineCourseWeek> OnlineCourseWeeks { get; set; }
        public DbSet<Homework> Homeworks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
	        modelBuilder.Entity<UserLesson>(entity =>
	        {
		        entity.HasKey(x => new { x.LessonId, x.UserId });
		        entity.HasOne(x => x.Lesson)
			        .WithMany(x => x.UserLessons)
			        .HasForeignKey(x => x.LessonId);
		        entity.HasOne(x => x.User)
			        .WithMany(x => x.UserLessons)
			        .HasForeignKey(x => x.UserId);
	        });
            modelBuilder.Entity<UserOnlineCourse>(entity =>
            {
                entity.HasKey(x => new { x.OnlineCourseId, x.UserId });
                entity.HasOne(x => x.OnlineCourse)
                    .WithMany(x => x.UserOnlineCourses)
                    .HasForeignKey(x => x.OnlineCourseId);
                entity.HasOne(x => x.User)
                    .WithMany(x => x.UserOnlineCourses)
                    .HasForeignKey(x => x.UserId);
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(x => new {x.Id }); 
                entity.HasOne(x => x.User)
                    .WithMany(x => x.Payments)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.Approver)
                    .WithMany(x => x.AprovedPayments)
                    .HasForeignKey(x => x.ApproverId)
                    .OnDelete(DeleteBehavior.NoAction);
              
            });
            modelBuilder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
	        modelBuilder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Roles"); });
            base.OnModelCreating(modelBuilder);
        }
    }

}
