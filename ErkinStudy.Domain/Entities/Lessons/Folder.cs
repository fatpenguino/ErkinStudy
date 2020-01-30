using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Lessons
{
    public class Folder
    {
	    public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
