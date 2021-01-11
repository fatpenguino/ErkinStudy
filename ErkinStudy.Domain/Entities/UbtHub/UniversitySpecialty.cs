namespace ErkinStudy.Domain.Entities.UbtHub
{
    public class UniversitySpecialty
    {
        public int UniversityId { get; set; }
        public int SpecialtyId { get; set; }
        public virtual University University { get; set; }
        public virtual Specialty Specialty { get; set; }
    }
}
