using System.Collections.Generic;
using System.Threading.Tasks;
using ErkinStudy.Domain.Models;

namespace ErkinStudy.Application.Repositories
{
    public interface ISubjectRepository
    {
        Task<Subject> GetAsync(long id);
        Task SaveAsync();
        void Add(Subject subject);
        Task<List<Subject>> GetAllAsync();
    }
}
