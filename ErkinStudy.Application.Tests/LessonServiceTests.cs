using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Application.Services;
using ErkinStudy.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ErkinStudy.Application.Tests
{
    public class LessonServiceTests
    {
        [Fact]
        public async Task AddLessonTest()
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
					var subject = await subjectService.AddSubjectAsync("test name", "test description");
                    var degree = await degreeService.AddDegreeAsync("test degree", "test degree description", 9, subject.Id);
                    var paragraph =  await paragraphService.AddParagraphAsync("test paragraph", "test paragraph description", subject.Id, degree.Id);
                    await lessonService.AddLessonAsync("test lesson", "test lesson description", 499, subject.Id, degree.Id, paragraph.Id);
                    await lessonService.AddLessonAsync("test lesson 2", "test lesson description 2", 599, subject.Id, degree.Id, paragraph.Id);
                    await lessonService.AddLessonAsync("test lesson 3", "test lesson description 3", 699, subject.Id, degree.Id, paragraph.Id);
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
                    Assert.Collection(paragraph?.Lessons ?? throw new InvalidOperationException(), item =>
	                    {
		                    Assert.Equal("test lesson", item.Name);
		                    Assert.Equal("test lesson description", item.Description);
							Assert.Equal(0, (int)item.Order);
							Assert.Equal(499, item.Price);
	                    },
	                    item =>
	                    {
		                    Assert.Equal("test lesson 2", item.Name);
		                    Assert.Equal("test lesson description 2", item.Description);
		                    Assert.Equal(1, (int) item.Order);
		                    Assert.Equal(599, item.Price);
	                    },
	                    item =>
	                    {
		                    Assert.Equal("test lesson 3", item.Name);
		                    Assert.Equal("test lesson description 3", item.Description);
		                    Assert.Equal(2, (int) item.Order);
		                    Assert.Equal(699, item.Price);
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