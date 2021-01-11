namespace ErkinStudy.Domain.Entities.UbtHub
{
    public class SpecialtySubject
    {
        public int SpecialtyId { get; set; }
        public short SubjectId { get; set; }
        public virtual Specialty Specialty { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
