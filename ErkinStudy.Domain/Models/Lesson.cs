using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Lesson
    {
	    public long Id { get; set; }
	    public long ParagraphId { get; set; }
	    public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint Order { get; set; }
        public int Price { get; set; }
        public virtual Paragraph Paragraph { get; set; }
        public virtual List<Content> Contents { get; set; }
    }
}
