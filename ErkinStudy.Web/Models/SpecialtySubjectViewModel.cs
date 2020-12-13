using System.Collections.Generic;

namespace ErkinStudy.Web.Models
{
    public class ManageSubjectViewModel
    {
        public int SpecialtyId { get; set; }
        public List<SubjectViewModel> Subjects { get; set; }
    }

    public class SubjectViewModel
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public bool IsMarked { get; set; }
    }
}
