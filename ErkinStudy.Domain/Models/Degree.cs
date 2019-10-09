using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Degree
    {
        public Degree(long id, uint level, string title, List<Paragraph> paragraphs)
        {
            Id = id;
            Level = level;
            Title = title;
            Paragraphs = paragraphs;
        }

        public long Id { get; set; }
        public uint Level { get; set; }
        public string Title { get; set; }
        public List<Paragraph> Paragraphs { get; set; }
    }
}
