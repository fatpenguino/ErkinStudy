using System;
using System.Collections.Generic;
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
	        var degree = new Degree()
	        {
		        Name = name,
		        Description = description,
				Subject = this,
				Level = level
	        };
			Degrees.Add(degree);
			return degree;
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
