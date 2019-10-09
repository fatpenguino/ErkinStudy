using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Paragraph
    {
        public Paragraph(long id, string title, string description, uint order, DateTime createdAt, List<Lesson> lessons)
        {
            Id = id;
            Title = title;
            Description = description;
            Order = order;
            CreatedAt = createdAt;
            Lessons = lessons;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public uint Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
