using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Lessons
{
    public class Lesson
    {
	    public long Id { get; set; }
	    public long FolderId { get; set; }
	    public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public uint Order { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual ICollection<Content> Contents { get; set; }

    }
}
