using System;
using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Application.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ErkinStudy.Application.Tests
{
    public class ParagraphServiceTests
    {
		/*
        [Fact]
        public async Task AddParagraphTest()
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
                    var subject = await subjectService.AddSubjectAsync("test name", "test description");
                    var degree = await degreeService.AddDegreeAsync("test degree", "test degree description", 9, subject.Id);
                    await paragraphService.AddParagraphAsync("test paragraph", "test paragraph description", subject.Id, degree.Id);
                    await paragraphService.AddParagraphAsync("test paragraph 2", "test paragraph description 2", subject.Id, degree.Id,1);
                    await paragraphService.AddParagraphAsync("test paragraph 3", "test paragraph description 3", subject.Id, degree.Id);
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
                    Assert.Collection(degree?.Paragraphs ?? throw new InvalidOperationException(), item =>
	                    {
		                    Assert.Equal("test paragraph", item.Name);
		                    Assert.Equal("test paragraph description", item.Description);
							Assert.Equal(0, (int)item.Order);
	                    },
	                    item =>
	                    {
		                    Assert.Equal("test paragraph 2", item.Name);
		                    Assert.Equal("test paragraph description 2", item.Description);
		                    Assert.Equal(1, (int) item.Order);
	                    },
	                    item =>
	                    {
		                    Assert.Equal("test paragraph 3", item.Name);
		                    Assert.Equal("test paragraph description 3", item.Description);
		                    Assert.Equal(2, (int) item.Order);
	                    });              
                }
            }
            finally
            {
                connection.Close();
            }
        }*/
    }
}