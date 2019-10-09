using System;
using System.Collections.Generic;
using System.Net.Mime;
using ErkinStudy.Domain.Models;
using ErkinStudy.Domain.Enums;
using Xunit;

namespace ErkinStudy.Domain.Tests.Models
{
    public class LessonTests
    {
        [Fact]
        public void Create()
        {
            var content = new Content(1, "Hello World", 1, ContentFormat.Text);
            var lesson = new Lesson(1, "Hello World", DateTime.UtcNow, 1, 500, new List<Content>() {content});
            Assert.NotNull(content);
            Assert.NotNull(lesson);
            Assert.NotEmpty(lesson.Contents);
        }
    }
}