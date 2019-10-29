using ErkinStudy.Domain.Entities;
using ErkinStudy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Content> Contents { get; set; }
    }

}
