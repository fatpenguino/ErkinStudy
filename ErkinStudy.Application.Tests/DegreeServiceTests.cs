using System.Linq;
using System.Threading.Tasks;
using ErkinStudy.Application.Services;
using ErkinStudy.Domain.Enums;
using ErkinStudy.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ErkinStudy.Application.Tests
{
    public class DegreeServiceTests
    {
        /*
	    [Fact]
        public async Task AddDegreeTest()
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
                    var subject = await subjectService.AddSubjectAsync("test name", "test description");
                    await degreeService.AddDegreeAsync("test degree", "test degree description", 9, subject.Id);
                    await degreeService.AddDegreeAsync("test degree 2", "test degree description 2", 10, subject.Id);
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
                    Assert.Collection(subject.Degrees, item =>
	                    {
		                    Assert.Equal("test degree", item.Name);
		                    Assert.Equal("test degree description", item.Description);
							Assert.Equal((uint)9, item.Level);
	                    },
	                    item =>
	                    {
		                    Assert.Equal("test degree 2", item.Name);
		                    Assert.Equal("test degree description 2", item.Description);
		                    Assert.Equal((uint) 10, item.Level);
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