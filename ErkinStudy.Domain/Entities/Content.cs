using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Entities
{
    public class Content
    {
        //for ef core
        public long Id { get; set; }
        public long LessonId { get; set; }
        public string Value { get; set; }
        public uint Order { get; set; }
        public bool IsActive { get; set; }
        public ContentFormat ContentFormat { get; set; }
        public virtual Lesson Lesson { get; set; }

    }
}
