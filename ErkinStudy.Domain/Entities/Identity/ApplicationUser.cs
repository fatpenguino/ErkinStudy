using System.Collections.Generic;
using ErkinStudy.Domain.Entities.Lessons;
using ErkinStudy.Domain.Entities.OnlineCourses;
using ErkinStudy.Domain.Entities.Quizzes;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserLesson> UserLessons { get; set; }
        public virtual ICollection<UserOnlineCourse> UserOnlineCourses { get; set; }
        public virtual ICollection<UserQuiz> UserQuizzes { get; set; }
    }
}
