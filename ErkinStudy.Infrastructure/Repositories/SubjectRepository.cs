using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Application;
using ErkinStudy.Application.Repositories;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Infrastructure.Repositories
{
    public class SubjectRepository : DbContext, ISubjectRepository
    {
        public SubjectRepository(DbContextOptions<SubjectRepository> options)
            : base(options)
        {
        }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Content> Contents { get; set; }

        public async Task<Subject> GetAsync(long id)
        {
            return await Subjects.Include(sub => sub.Degrees)
                .ThenInclude(deg => deg.Paragraphs)
                .ThenInclude(les => les.Lessons)
                .ThenInclude(c => c.Contents)
                .Where(sub => sub.Id == id).SingleOrDefaultAsync();
        }
        
        public Task SaveAsync()
        {
            return SaveChangesAsync();
        }
        public void Add(Subject subject)
        {
            Subjects.Add(subject);
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await Subjects.Include(sub => sub.Degrees)
                .ThenInclude(deg => deg.Paragraphs)
                .ThenInclude(les => les.Lessons)
                .ThenInclude(c => c.Contents).Where(x => x.State != SubjectState.Deleted).ToListAsync();
        }
    }
}
