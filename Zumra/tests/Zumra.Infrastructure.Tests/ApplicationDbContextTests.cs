using Microsoft.EntityFrameworkCore;
using Zumra.Domain.Entities;
using Zumra.Infrastructure.Persistence;

namespace Zumra.Infrastructure.Tests
{
    public class ApplicationDbContextTests
    {
        [Fact]
        public async Task SaveChangesAsync_ShouldSaveChanges_WhenValidEntityIsAdded()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var todoItem = new TodoItem
                {
                    Title = "Test Todo",
                    Description = "Test Description",
                    Status = TodoStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                context.TodoItems.Add(todoItem);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var savedItem = await context.TodoItems.FirstOrDefaultAsync();

                Assert.NotNull(savedItem);
                Assert.Equal("Test Todo", savedItem.Title);
                Assert.Equal("Test Description", savedItem.Description);
                Assert.Equal(TodoStatus.Pending, savedItem.Status);
            }
        }

        [Fact]
        public async Task TodoItems_ShouldReturnEmptyList_WhenNoItemsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var items = await context.TodoItems.ToListAsync();

                Assert.Empty(items);
            }
        }
    }
}