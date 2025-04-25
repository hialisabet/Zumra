using MediatR;
using ZumraTask.Application.Dtos;

namespace ZumraTask.Application.Commands;

public record CreateToDoItemCommand(string Title, string Description)
    : IRequest<ToDoItemDto>;
