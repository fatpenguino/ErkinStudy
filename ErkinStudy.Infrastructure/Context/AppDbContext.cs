﻿using ErkinStudy.Domain.Entities.Identity;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Domain.Entities.Payment;
using ErkinStudy.Domain.Entities.Quizzes;
using ErkinStudy.Domain.Entities.UbtHub;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, IdentityUserClaim<long>, ApplicationUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>> {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<OnlineCourse> OnlineCourses { get; set; }
        public DbSet<OnlineCourseWeek> OnlineCourseWeeks { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizScore> QuizScores { get; set; }
        public DbSet<UserFolder> UserFolders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderOperation> OrderOperations { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<SpecialtySubject> SpecialtySubjects { get; set; }
        public DbSet<UniversitySpecialty> UniversitySpecialties { get; set; }
        public DbSet<City> Cities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
	        modelBuilder.Entity<UserFolder>(entity =>
	        {
		        entity.HasKey(x => new { x.FolderId, x.UserId });
		        entity.HasOne(x => x.Folder)
			        .WithMany(x => x.UserFolders)
			        .HasForeignKey(x => x.FolderId);
		        entity.HasOne(x => x.User)
			        .WithMany(x => x.UserFolders)
			        .HasForeignKey(x => x.UserId);
	        });

            modelBuilder.Entity<SpecialtySubject>(entity =>
            {
                entity.HasKey(x => new { x.SpecialtyId, x.SubjectId });
                entity.HasOne(x => x.Specialty)
                    .WithMany(x => x.SpecialtySubjects)
                    .HasForeignKey(x => x.SpecialtyId);
                entity.HasOne(x => x.Subject)
                    .WithMany(x => x.SpecialtySubjects)
                    .HasForeignKey(x => x.SubjectId);
            });

            modelBuilder.Entity<UniversitySpecialty>(entity =>
            {
                entity.HasKey(x => new { x.UniversityId, x.SpecialtyId });
                entity.HasOne(x => x.University)
                    .WithMany(x => x.UniversitySpecialties)
                    .HasForeignKey(x => x.UniversityId);
                entity.HasOne(x => x.Specialty)
                    .WithMany(x => x.UniversitySpecialties)
                    .HasForeignKey(x => x.SpecialtyId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
