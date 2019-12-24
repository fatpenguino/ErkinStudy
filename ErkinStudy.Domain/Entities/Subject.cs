using System.Collections.Generic;

namespace ErkinStudy.Domain.Entities
{
    public class Subject
    {
        // for ef core
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual List<Folder> Folders { get; set; }
    }
}
