using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Domain.Entities.UbtHub;
using ErkinStudy.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ErkinStudy.Infrastructure.Services
{
    public class SpecialtyService
    {
        private readonly AppDbContext _context;

        public SpecialtyService(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Specialty>> GetAll()
        {
          return _context.Specialties.Include(x => x.SpecialtySubjects).ThenInclude(x => x.Subject).ToListAsync();
        }
        public async Task<List<Subject>> GetSubjects(int id)
        {
            return await _context.SpecialtySubjects.Include(x => x.Subject).Where(x => x.SpecialtyId == id)
                .Select(x => x.Subject).ToListAsync();
        }

        public async Task<List<Subject>> GetExcludedSubjects(int id)
        {
            var included = await GetSubjects(id);
            return await _context.Subjects.Where(x => !included.Contains(x)).ToListAsync();
        }

        public async Task<List<University>> GetUniversities(int id)
        {
            return await _context.UniversitySpecialties.Include(x => x.University).Where(x => x.SpecialtyId == id)
                .Select(x => x.University).ToListAsync();
        }
        public async Task<List<University>> GetExcludedUniversities(int id)
        {
            var included = await GetUniversities(id);
            return await _context.Universities.Where(x => !included.Contains(x)).ToListAsync();
        }
    }
}
