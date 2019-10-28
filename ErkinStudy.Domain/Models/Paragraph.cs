using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Paragraph
    {
	    public long Id { get; set; }
	    public long DegreeId { get; set; }
	    public string Name { get; set; }
        public string Description { get; set; }
        public uint Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<Lesson> Lessons { get; set; }
        public virtual Degree Degree { get; set; }
    }
}
