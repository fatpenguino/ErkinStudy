using System;
using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.OnlineCourses
{
    public class OnlineCourse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? FolderId { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<OnlineCourseWeek> OnlineCourseWeeks { get; set; }
        public virtual ICollection<UserOnlineCourse> UserOnlineCourses { get; set; }
    }
}
