using System.Threading.Tasks;
using ErkinStudy.Application.Repositories;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Domain.Models;

namespace ErkinStudy.Application.Services
{
    public class ContentService
    {
        private readonly ISubjectRepository _subjectRepository;

        public ContentService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<Content> AddContentAsync(string value, ContentFormat format, long subjectId, long degreeId, long paragraphId, long lessonId, int? order = null)
        {
	        var subject = await _subjectRepository.GetAsync(subjectId);
	        var content = subject.AddContent(value, format, degreeId, paragraphId, lessonId,order);
            await _subjectRepository.SaveAsync();
            return content;
        }
    }
}
