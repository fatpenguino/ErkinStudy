using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities
{
    public class Degree
    {
	    public long Id { get; set; }
	    public long SubjectId { get; set; }
	    public uint Level { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Paragraph> Paragraphs { get; set; }
    }
}
