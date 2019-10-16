using System;
using System.Collections.Generic;
using System.Linq;
using ErkinStudy.Domain.Enums;

namespace ErkinStudy.Domain.Models
{
    public class Subject
    {
        // for ef core
        public Subject()
        { }

		public Subject(string name, string description)
        {
            Name = name;
            Description = description;
            Degrees = new List<Degree>();
        }

        public void UpdateInfo(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is not specified", nameof(name));
            }

            Name = name;
            Description = description;
        }

        public Degree AddDegree(string name, string description, uint level)
        {
	        var degree = new Degree(name, description, this, level);
	        Degrees.Add(degree);
			return degree;
        }

        public Paragraph AddParagraph(string name, string description, long degreeId, int? order = null)
        {
	        var degree = Degrees.Find(x => x.Id == degreeId);
	        order ??= degree.Paragraphs.Any() ? (int)degree.Paragraphs.Max(x => x.Order) + 1 : 0;
			var paragraph = new Paragraph(name, description, degree, (uint)order, DateTime.UtcNow);
	        degree.Paragraphs.Add(paragraph);
	        return paragraph;
        }

        public Lesson AddLesson(string name, string description, int price, long degreeId, long paragraphId, int? order = null)
        {
	        var degree = Degrees.Find(x => x.Id == degreeId);
	       var paragraph = degree.Paragraphs.Find(x => x.Id == paragraphId);
	       order ??= paragraph.Lessons.Any() ? (int) paragraph.Lessons.Max(x => x.Order) + 1 : 0;
	       var lesson = new Lesson(name, description, paragraph, DateTime.UtcNow, (uint)order, price);
	       paragraph.Lessons.Add(lesson);
	       return lesson;
        }

        public void UpdateState(SubjectState state)
        {
            State = state;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectState State { get; set; }
        public List<Degree> Degrees { get; set; }
    }
}
