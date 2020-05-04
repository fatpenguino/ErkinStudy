using ErkinStudy.Domain.Entities.Identity;

namespace ErkinStudy.Domain.Entities.Lessons
{
	public class UserFolder
    {
	    public long UserId { get; set; }
        public long FolderId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
