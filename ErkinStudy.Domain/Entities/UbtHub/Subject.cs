using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.UbtHub
{
    public class Subject
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public ICollection<SpecialtySubject> SpecialtySubjects { get; set; }
    }
}
