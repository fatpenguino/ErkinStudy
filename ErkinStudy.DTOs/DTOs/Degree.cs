using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Degree
    {
        //for ef core
        public Degree()
        { }
        public Degree(string name, string description, Subject subject, uint level)
        {
            Level = level;
            Name = name;
            Description = description;
            Subject = subject;
            Paragraphs = new List<Paragraph>();
        }

        public long Id { get; set; }
        public uint Level { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Subject Subject { get; set; }
        public List<Paragraph> Paragraphs { get; set; }
    }
}
