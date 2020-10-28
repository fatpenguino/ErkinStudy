using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.Lessons
{
    public class Folder
    {
	    public long Id { get; set; }
        public long? ParentId { get; set; }
        public long? TeacherId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public long Price { get; set; }
        public string LandingPage { get; set; }
        public bool EnableLanding { get; set; }
        public string Color { get; set; }
        public bool IsQuizGroup { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<UserFolder> UserFolders { get; set; }
    }
}
