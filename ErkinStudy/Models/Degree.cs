using System.Collections.Generic;

namespace ErkinStudy.Models
{
    public class Degree
    {
        public Degree(List<Paragraph> paragraphs, string title, uint level, long id)
        {
            Paragraphs = paragraphs;
            Title = title;
            Level = level;
            Id = id;
        }

        public long Id { get; set; }
        public uint Level { get; set; }
        public string Title { get; set; }
        public List<Paragraph> Paragraphs { get; set; }
    }
}
