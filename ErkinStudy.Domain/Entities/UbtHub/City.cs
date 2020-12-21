using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities.UbtHub
{
    public class City
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<University> Universities { get; set; }
    }
}
