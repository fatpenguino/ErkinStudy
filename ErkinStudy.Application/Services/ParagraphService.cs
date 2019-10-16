using System;
using System.Threading.Tasks;
using ErkinStudy.Application.Repositories;
using ErkinStudy.Domain.Models;

namespace ErkinStudy.Application.Services
{
    public class ParagraphService
    {
        private readonly ISubjectRepository _subjectRepository;

        public ParagraphService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<Paragraph> AddParagraphAsync(string name, string description, long subjectId, long degreeId, int? order = null)
        {
	        var subject = await _subjectRepository.GetAsync(subjectId);
	        var paragraph = subject.AddParagraph(name, description, degreeId, order);
			paragraph.CreatedAt = DateTime.UtcNow;
			await _subjectRepository.SaveAsync();
            return paragraph;
        }
    }
}
