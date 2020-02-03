using ErkinStudy.Domain.Entities.Identity;

namespace ErkinStudy.Domain.Entities.OnlineCourses
{
	public class UserOnlineCourse
    {
	    public long UserId { get; set; }
        public long OnlineCourseId { get; set; }
        public bool IsActive { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual OnlineCourse OnlineCourse { get; set; }
    }
}
