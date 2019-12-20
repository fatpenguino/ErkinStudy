using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Payment> AprovedPayments { get; set; }
        public virtual ICollection<UserLesson> UserLessons { get; set; }
        public virtual ICollection<UserOnlineCourse> UserOnlineCourses { get; set; }
    }
}
