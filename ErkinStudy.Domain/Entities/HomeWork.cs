using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities
{
    public class Homework
    {
        public long Id { get; set; }
        public long OnlineCourseWeekId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public virtual OnlineCourseWeek OnlineCourseWeek { get; set; }
    }
}
