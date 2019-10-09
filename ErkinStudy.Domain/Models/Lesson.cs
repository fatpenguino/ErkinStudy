using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Lesson
    {
        public Lesson(long id, string title, DateTime createdAt, uint order, int price, List<Content> contents)
        {
            Id = id;
            Title = title;
            CreatedAt = createdAt;
            Order = order;
            Price = price;
            Contents = contents;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint Order { get; set; }
        public int Price { get; set; }
        public List<Content> Contents { get; set; }
    }
}
