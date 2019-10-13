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
    public class SubjectServiceTests
    {
        [Fact]
        public async Task AddSubjectTest()
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
                    var subject = await subjectService.AddSubjectAsync("test name", "test description");

                    Assert.NotEqual(0, subject.Id);
                }

                // Use a separate instance of the context to verify correct data was saved to database
                await using (var context = new SubjectRepository(options))
                {
                    var subject = await context.GetAsync(1);
                    Assert.Equal(1, context.Subjects.Count());
                    Assert.Equal("test name", subject.Name);
                    Assert.Equal("test description", subject.Description);
                    Assert.NotEqual(0, subject.Id);
                    Assert.NotNull(subject.Degrees);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async Task GetAllTest()
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
                    await subjectService.AddSubjectAsync("test name 1", "test description 1");
                    await subjectService.AddSubjectAsync("test name 2", "test description 2");
                }

                // Use a separate instance of the context to verify correct data was saved to database
                await using (var context = new SubjectRepository(options))
                {
                    var subjectService = new SubjectService(context);
                    var subjects = await subjectService.GetAllAsync();

                    Assert.Collection(subjects, subject =>
                        {
                            Assert.Equal(1, subject.Id);
                            Assert.Equal("test name 1", subject.Name);
                            Assert.Equal("test description 1", subject.Description);
                        },
                        subject =>
                        {
                            Assert.Equal(2, subject.Id);
                            Assert.Equal("test name 2", subject.Name);
                            Assert.Equal("test description 2", subject.Description);
                        });
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async Task GetTest()
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

                await using (var repository = new SubjectRepository(options))
                {
                    var subjectService = new SubjectService(repository);
                    await subjectService.AddSubjectAsync("name", "desc");
                }

                // Use a separate instance of the context to verify correct data was saved to database
                await using (var repository = new SubjectRepository(options))
                {
                    var subjectService = new SubjectService(repository);
                    var subject = await subjectService.GetAsync(1);

                    Assert.Equal(1, subject.Id);
                    Assert.Equal("name", subject.Name);
                    Assert.Equal("desc", subject.Description);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async Task RemoveTest()
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
                    await subjectService.AddSubjectAsync("test name 1", "test description 1");
                }

                // Use a separate instance of the context to verify correct data was saved to database
                await using (var context = new SubjectRepository(options))
                {
                    var subjectService = new SubjectService(context);
                    await subjectService.RemoveAsync(1);
                    var subject = subjectService.GetAsync(1).Result;
                    Assert.NotNull(subject);
                    Assert.True(subject.State == SubjectState.Deleted);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}