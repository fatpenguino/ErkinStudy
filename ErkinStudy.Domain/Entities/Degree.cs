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
        public virtual Subject Subject { get; set; }
        public virtual List<Paragraph> Paragraphs { get; set; }
    }
}
