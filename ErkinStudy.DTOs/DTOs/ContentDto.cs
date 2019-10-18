using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Models
{
    public class ContentDto
    {
        

        public long Id { get; set; }
        public string Value { get; set; }
        public uint Order { get; set; }
        public ContentFormat ContentFormat { get; set; }
    }
}
