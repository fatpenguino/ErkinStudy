using System;
using ErkinStudy.Domain.Models;
using ErkinStudy.Domain.Enums;
using Xunit;

namespace ErkinStudy.Domain.Tests.Models
{
    public class SubjectTests
    {
        [Fact]
        public void Create()
        {
	        var subject = new Subject("Introduction", "Algorithms and Structures");
	        var degree = new Degree("class", "desc", subject, 9);
	        var paragraph = new Paragraph("Introduction to programming", "C# Language Introduction", degree, 1, DateTime.UtcNow);
	        var lesson = new Lesson("Hello World", "Desc", paragraph, DateTime.UtcNow, 1, 500);
            var content = new Content("Hello World", lesson, 1, ContentFormat.Text);
            Assert.NotNull(content);
            Assert.NotNull(lesson);
            Assert.NotNull(paragraph);
            Assert.NotNull(degree);
            Assert.NotNull(subject);
        }
    }
}