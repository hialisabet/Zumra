using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Zumra.API.Controllers;
using Zumra.Application.Features.TodoItems.Commands;
using Zumra.Application.Features.TodoItems.Queries;

namespace Zumra.API.Tests
{
    public class TodoItemsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TodoItemsController _controller;

        public TodoItemsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TodoItemsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithAllTodoItems()
        {
            // Arrange
            var todoItems = new List<TodoItemDto>
            {
                new TodoItemDto { Id = 1, Title = "Task 1" },
                new TodoItemDto { Id = 2, Title = "Task 2" }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTodoItems.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItems);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItems = Assert.IsType<List<TodoItemDto>>(okResult.Value);
            Assert.Equal(2, returnedItems.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult_WhenItemExists()
        {
            // Arrange
            var todoItem = new TodoItemDto { Id = 1, Title = "Task 1" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTodoItemById.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItem);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedItem = Assert.IsType<TodoItemDto>(okResult.Value);
            Assert.Equal(1, returnedItem.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTodoItemById.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TodoItemDto)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WithNewId()
        {
            // Arrange
            var command = new CreateTodoItem.Command { Title = "New Task", Description = "Description" };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var command = new UpdateTodoItem.Command { Id = 1, Title = "Updated Task" };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(1, command);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var command = new UpdateTodoItem.Command { Id = 1, Title = "Updated Task" };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(1, command);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTodoItem.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTodoItem.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}