using System.Collections.Generic;
using System.Threading.Tasks;
using ErkinStudy.Application.Repositories;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Domain.Models;

namespace ErkinStudy.Application.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<Subject> AddSubjectAsync(string name, string description)
        {
            var subject = new Subject(name, description);

            _subjectRepository.Add(subject);
            await _subjectRepository.SaveAsync();

            return subject;
        }

        /// <summary>
        /// Get all existing subjects
        /// </summary>
        /// <returns>List of subjects dto</returns>
        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            return subjects;
        }

        /// <summary>
        /// Get subject
        /// </summary>
        /// <param name="id">Subject id</param>
        /// <returns></returns>
        public async Task<Subject> GetAsync(long id)
        {
            var subject = await _subjectRepository.GetAsync(id);
            return subject;
        }

        /// <summary>
        /// Update subject info
        /// </summary>
        /// <param name="id">Subject id</param>
        /// <param name="name">Subject name</param>
        /// <param name="description">Subject description</param>
        /// <returns></returns>
        public async Task UpdateSubject(long id, string name, string description)
        {
            var subject = await _subjectRepository.GetAsync(id);
            subject.UpdateInfo(name, description);

            await _subjectRepository.SaveAsync();
        }
        /// <summary>
        /// Remove subject aggregate
        /// </summary>
        /// <param name="id">Subject id</param>
        /// <returns></returns>
        public async Task RemoveAsync(long id)
        {
            var subject = await _subjectRepository.GetAsync(id);
            subject.UpdateState(SubjectState.Deleted);

            await _subjectRepository.SaveAsync();

        }
    }
}
