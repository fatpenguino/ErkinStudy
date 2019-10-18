using System;
using System.Collections.Generic;
using System.Linq;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Models
{
    public class Subject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectState State { get; set; }
        public List<Degree> Degrees { get; set; }
    }
}
