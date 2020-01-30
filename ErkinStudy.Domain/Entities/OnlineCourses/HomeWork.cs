using System;

namespace ErkinStudy.Domain.Entities.OnlineCourses
{
    public class Homework
    {
        public long Id { get; set; }
        public long OnlineCourseWeekId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime UploadTime { get; set; }
        public virtual OnlineCourseWeek OnlineCourseWeek { get; set; }
    }
}
