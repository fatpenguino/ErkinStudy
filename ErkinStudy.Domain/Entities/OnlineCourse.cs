using System;
using System.Collections.Generic;
using System.Text;

namespace ErkinStudy.Domain.Entities
{
    public class OnlineCourse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfWeeks { get; set; }
        public long Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<OnlineCourseWeek> OnlineCourseWeeks { get; set; }
        public virtual ICollection<UserOnlineCourse> UserOnlineCourses { get; set; }
    }
}
