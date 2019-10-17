using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Application.Services;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ErkinStudy.Application.Tests
{
    public class ContentServiceTests
    {
        [Fact]
        public async Task AddContentTest()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<SubjectRepository>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                await using (var context = new SubjectRepository(options))
                {
                    context.Database.EnsureCreated();
                }

                // Run the test against one instance of the context
                await using (var repository = new SubjectRepository(options))
                {
                    var subjectService = new SubjectService(repository);
					var degreeService = new DegreeService(repository);
					var paragraphService = new ParagraphService(repository);
                    var lessonService = new LessonService(repository);
                    var contentService = new ContentService(repository);
					var subject = await subjectService.AddSubjectAsync("test name", "test description");
                    var degree = await degreeService.AddDegreeAsync("test degree", "test degree description", 9, subject.Id);
                    var paragraph =  await paragraphService.AddParagraphAsync("test paragraph", "test paragraph description", subject.Id, degree.Id);
                    var lesson =  await lessonService.AddLessonAsync("test lesson", "test lesson description", 499, subject.Id, degree.Id, paragraph.Id);
                    await contentService.AddContentAsync("test value", ContentFormat.Text, subject.Id, degree.Id,
                        paragraph.Id, lesson.Id);
                    await contentService.AddContentAsync("test value 2", ContentFormat.VideoLink, subject.Id, degree.Id,
                        paragraph.Id, lesson.Id);
                    await contentService.AddContentAsync("test value 3", ContentFormat.Image, subject.Id, degree.Id,
                        paragraph.Id, lesson.Id);
                }

                // Use a separate instance of the context to verify correct data was saved to database
                await using (var context = new SubjectRepository(options))
                {
                    var subject = await context.GetAsync(1);
                    Assert.Equal(1, context.Subjects.Count());
                    Assert.Equal("test name", subject.Name);
                    Assert.Equal("test description", subject.Description);
                    Assert.NotEqual(0, subject.Id);
                    Assert.NotEmpty(subject.Degrees);
					var degree = subject.Degrees.FirstOrDefault();
                    Assert.Equal("test degree", degree?.Name);
                    Assert.Equal("test degree description", degree?.Description);
                    Assert.NotEqual(0, degree?.Id);
                    var paragraph = degree?.Paragraphs.FirstOrDefault();
                    Assert.Equal("test paragraph", paragraph?.Name);
                    Assert.Equal("test paragraph description", paragraph?.Description);
                    Assert.NotEqual(0, paragraph?.Id);
                    var lesson = paragraph?.Lessons.FirstOrDefault();
                    Assert.Equal("test lesson", lesson?.Name);
                    Assert.Equal("test lesson description", lesson?.Description);
                    Assert.NotEqual(0, lesson?.Id);
                    Assert.Collection(lesson?.Contents ?? throw new InvalidOperationException(), item =>
	                    {
		                    Assert.Equal("test value", item.Value);
		                    Assert.Equal(ContentFormat.Text, item.ContentFormat);
							Assert.Equal(0, (int)item.Order);
	                    },
	                    item =>
	                    {
                            Assert.Equal("test value 2", item.Value);
                            Assert.Equal(ContentFormat.VideoLink, item.ContentFormat);
                            Assert.Equal(1, (int)item.Order);
                        },
	                    item =>
	                    {
                            Assert.Equal("test value 3", item.Value);
                            Assert.Equal(ContentFormat.Image, item.ContentFormat);
                            Assert.Equal(2, (int)item.Order);
                        });              
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}