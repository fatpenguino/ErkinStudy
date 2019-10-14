using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Lesson
    {
        //for ef core
        public Lesson()
        { }
        public Lesson(string name, string description, Paragraph paragraph, DateTime createdAt, uint order, int price)
        {
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            Order = order;
            Price = price;
            Paragraph = paragraph;
            Contents = new List<Content>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Paragraph Paragraph { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint Order { get; set; }
        public int Price { get; set; }
        public List<Content> Contents { get; set; }
    }
}
