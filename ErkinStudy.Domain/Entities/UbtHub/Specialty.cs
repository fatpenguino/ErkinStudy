using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.UbtHub
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
        public ICollection<SpecialtySubject> SpecialtySubjects { get; set; }
        public ICollection<UniversitySpecialty> UniversitySpecialties { get; set; }
    }
}
