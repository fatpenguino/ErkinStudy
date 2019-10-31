using ErkinStudy.Domain.Entities.Identity;

namespace ErkinStudy.Domain.Entities
{
	public class UserLesson
    {
	    public long UserId { get; set; }
        public long LessonId { get; set; }
        public bool IsActive { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
