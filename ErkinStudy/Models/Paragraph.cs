using System;
using System.Collections.Generic;

namespace ErkinStudy.Models
{
    public class Paragraph
    {
        public Paragraph(List<Lesson> lessons, DateTime createdAt, uint order, string description, string title, long id)
        {
            Lessons = lessons;
            CreatedAt = createdAt;
            Order = order;
            Description = description;
            Title = title;
            Id = id;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public uint Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
