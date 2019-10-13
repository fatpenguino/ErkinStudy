using System;
using System.Collections.Generic;
using System.Net.Mime;
using ErkinStudy.Domain.Models;
using ErkinStudy.Domain.Enums;
using Xunit;

namespace ErkinStudy.Domain.Tests.Models
{
    public class ParagraphTests
    {
        [Fact]
        public void Create()
        {
            var content = new Content( "Hello World", 1, ContentFormat.Text);
            var lesson = new Lesson("Hello World", "", DateTime.UtcNow, 1, 500);
            var paragraph = new Paragraph("Introduction to programming", "C# Language Introduction", 1, DateTime.UtcNow);
            Assert.NotNull(content);
            Assert.NotNull(lesson);
            Assert.NotNull(paragraph);
        }
    }
}