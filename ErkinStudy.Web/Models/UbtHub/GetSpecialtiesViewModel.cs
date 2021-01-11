using System.Collections.Generic;

namespace ErkinStudy.Web.Models.UbtHub
{
    public class GetSpecialtiesViewModel
    {
        public string FirstSubject { get; set; }
        public string SecondSubject { get; set; }
        public List<string> Universities { get; set; } = new List<string>();
    }
}
