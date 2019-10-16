using System.Threading.Tasks;
using ErkinStudy.Application.Repositories;
using ErkinStudy.Domain.Models;

namespace ErkinStudy.Application.Services
{
    public class LessonService
    {
        private readonly ISubjectRepository _subjectRepository;

        public LessonService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<Lesson> AddLessonAsync(string name, string description, int price, long subjectId, long degreeId, long paragraphId, int? order = null)
        {
	        var subject = await _subjectRepository.GetAsync(subjectId);
	        var lesson = subject.AddLesson(name, description, price, degreeId, paragraphId, order);
            await _subjectRepository.SaveAsync();
            return lesson;
        }
    }
}
