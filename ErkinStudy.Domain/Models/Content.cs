using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Models
{
    public class Content
    {
        //for ef core
        public Content()
        {
        }

        public Content(string value, uint order, ContentFormat contentFormat)
        {
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
