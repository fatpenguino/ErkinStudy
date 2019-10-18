using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Models
{
    public class Lesson
    {
        //for ef core
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Paragraph Paragraph { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint Order { get; set; }
        public int Price { get; set; }
        public List<ContentDto> Contents { get; set; }
    }
}
