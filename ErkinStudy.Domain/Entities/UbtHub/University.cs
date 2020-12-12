using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.UbtHub
{
    public class University
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public ICollection<UniversitySpecialty> UniversitySpecialties { get; set; }
    }
}
