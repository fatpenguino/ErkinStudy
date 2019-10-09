using System;
using System.Collections.Generic;
using System.Text;

namespace ErkinStudy.Models
{
    public class Lesson
    {
        public Lesson(List<Content> contents, int price, uint order, DateTime createdAt, string title, long id)
        {
            Contents = contents;
            Price = price;
            Order = order;
            CreatedAt = createdAt;
            Title = title;
            Id = id;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint Order { get; set; }
        public int Price { get; set; }
        public List<Content> Contents { get; set; }
    }
}
