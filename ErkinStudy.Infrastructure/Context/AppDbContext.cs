using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Domain.Entities.Quizzes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long> {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<UserLesson> UserLessons { get; set; }
        public DbSet<UserOnlineCourse> UserOnlineCourses { get; set; }
        public DbSet<OnlineCourse> OnlineCourses { get; set; }
        public DbSet<OnlineCourseWeek> OnlineCourseWeeks { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizScore> QuizScores { get; set; }
        public DbSet<UserQuiz> UserQuizzes { get; set; }
        public DbSet<Category> Categories { get; set; }

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
            modelBuilder.Entity<UserQuiz>(entity =>
            {
                entity.HasKey(x => new { x.QuizId, x.UserId });
                entity.HasOne(x => x.Quiz)
                    .WithMany(x => x.UserQuizzes)
                    .HasForeignKey(x => x.QuizId);
                entity.HasOne(x => x.User)
                    .WithMany(x => x.UserQuizzes)
                    .HasForeignKey(x => x.UserId);
            });
            modelBuilder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
	        modelBuilder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Roles"); });
            base.OnModelCreating(modelBuilder);
        }
    }

}
