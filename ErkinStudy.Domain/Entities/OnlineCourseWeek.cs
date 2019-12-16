using System;
using System.Collections.Generic;
using System.Text;

namespace ErkinStudy.Domain.Entities
{
    public class OnlineCourseWeek
    {
        public long Id { get; set; }
        public long OnlineCourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int Order { get; set; }
        public string StreamUrl { get; set; }
        public virtual OnlineCourse OnlineCourse { get; set; }

    }
}
