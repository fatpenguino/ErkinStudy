using System.Collections.Generic;
using System.Threading.Tasks;
using ErkinStudy.Application.Repositories;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Domain.Models;

namespace ErkinStudy.Application.Services
{
    public class DegreeService
    {
        private readonly ISubjectRepository _subjectRepository;

        public DegreeService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<Degree> AddDegreeAsync(string name, string description, uint level, long subjectId)
        {
	        var subject = await _subjectRepository.GetAsync(subjectId);
	        var degree = subject.AddDegree(name, description, level);
            await _subjectRepository.SaveAsync();
            return degree;
        }
    }
}
