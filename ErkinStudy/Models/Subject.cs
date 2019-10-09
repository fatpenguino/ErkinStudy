using System;
using System.Collections.Generic;
using System.Text;

namespace ErkinStudy.Models
{
    public class Subject
    {
        public Subject(long id, string title, List<Degree> degrees)
        {
            Id = id;
            Title = title;
            Degrees = degrees;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public List<Degree> Degrees { get; set; }
    }
}
