using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Paragraph
    {
        //for ef core
        public Paragraph()
        {
        }

        public Paragraph(string name, string description, uint order, DateTime createdAt)
        {
            Name = name;
            Description = description;
            Order = order;
            CreatedAt = createdAt;
            Lessons = new List<Lesson>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
