using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.UbtHub
{
    public class University
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string ShortTitle { get; set; }
        public short CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<UniversitySpecialty> UniversitySpecialties { get; set; }
    }
}
