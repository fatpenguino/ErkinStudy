using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.OnlineCourses
{
    public class OnlineCourseWeek
    {
        public long Id { get; set; }
        public long OnlineCourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string StreamUrl { get; set; }
        public virtual OnlineCourse OnlineCourse { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }

    }
}
