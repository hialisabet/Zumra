using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Zumra.Application.Features.TodoItems.Commands;
using Zumra.Application.Interfaces;
using Zumra.Domain.Entities;

namespace Zumra.Application.Tests
{
    public class CreateTodoItemTests
    {
        [Fact]
        public async Task Handle_ShouldCreateTodoItem_WhenValidRequest()
        {
            // Arrange
            var todoItems = new List<TodoItem>();
            var mockDbSet = new Mock<DbSet<TodoItem>>();
            mockDbSet.Setup(m => m.Add(It.IsAny<TodoItem>())).Callback<TodoItem>(todoItems.Add);

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.TodoItems).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new CreateTodoItem.Command
            {
                Title = "Test Todo",
                Description = "Test Description"
            };

            var handler = new CreateTodoItem.Handler(mockContext.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
            Assert.Single(todoItems);
            Assert.Equal(command.Title, todoItems[0].Title);
            Assert.Equal(command.Description, todoItems[0].Description);
            Assert.Equal(TodoStatus.Pending, todoItems[0].Status);
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}