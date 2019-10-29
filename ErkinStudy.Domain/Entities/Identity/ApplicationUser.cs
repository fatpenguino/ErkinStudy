using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual List<Payment> Payments { get; set; }
        public virtual List<UserLesson> UserLessons { get; set; }
    }
}
