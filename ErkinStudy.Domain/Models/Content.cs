using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Models
{
    public class Content
    {
        public Content(long id, string value, uint order, ContentFormat contentFormat)
        {
            Id = id;
            Value = value;
            Order = order;
            ContentFormat = contentFormat;
        }

        public long Id { get; set; }
        public string Value { get; set; }
        public uint Order { get; set; }
        public ContentFormat ContentFormat { get; set; }
    }
}
