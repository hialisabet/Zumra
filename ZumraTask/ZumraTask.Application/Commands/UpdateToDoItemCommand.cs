using MediatR;

namespace ZumraTask.Application.Commands;

public record UpdateToDoItemCommand(int Id, string Title, string Description, string Status) : IRequest;