using System.Net.Mime;
using ErkinStudy.Domain.Models;
using ErkinStudy.Domain.Enums;
using Xunit;

namespace ErkinStudy.Domain.Tests.Models
{
    public class ContentTests
    {
        [Fact]
        public void Create()
        {
            var content = new Content(1, "Hello World", 1, ContentFormat.Text);
            Assert.NotNull(content);
        }
    }
}