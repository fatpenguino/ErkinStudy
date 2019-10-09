using System;
using System.Collections.Generic;
using System.Net.Mime;
using ErkinStudy.Domain.Models;
using ErkinStudy.Domain.Enums;
using Xunit;

namespace ErkinStudy.Domain.Tests.Models
{
    public class DegreeTests
    {
        [Fact]
        public void Create()
        {
            var content = new Content(1, "Hello World", 1, ContentFormat.Text);
            var lesson = new Lesson(1, "Hello World", DateTime.UtcNow, 1, 500, new List<Content>() {content});
            var paragraph = new Paragraph(1, "Introduction to programming", "C# Language Introduction", 1, DateTime.UtcNow, new List<Lesson>() { lesson});
            var degree = new Degree(1, 8, "class", new List<Paragraph>() { paragraph});
            Assert.NotNull(content);
            Assert.NotNull(lesson);
            Assert.NotNull(paragraph);
            Assert.NotNull(degree);
            Assert.NotEmpty(degree.Paragraphs);
        }
    }
}