using System.Collections.Generic;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Domain.Entities.Quizzes;

namespace ErkinStudy.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<OnlineCourse> OnlineCourses { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}
