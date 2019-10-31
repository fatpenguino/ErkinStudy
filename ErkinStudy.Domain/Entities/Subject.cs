using System.Collections.Generic;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Entities
{
    public class Subject
    {
        // for ef core
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectState State { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Degree> Degrees { get; set; }
    }
}
