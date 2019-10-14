using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Models
{
    public class Content
    {
        //for ef core
        public Content()
        {
        }

        public Content(string value, Lesson lesson, uint order, ContentFormat contentFormat)
        {
            Value = value;
            Lesson = lesson;
            Order = order;
            ContentFormat = contentFormat;
        }

        public long Id { get; set; }
        public string Value { get; set; }
        public Lesson Lesson { get; set; }
        public uint Order { get; set; }
        public ContentFormat ContentFormat { get; set; }
    }
}
