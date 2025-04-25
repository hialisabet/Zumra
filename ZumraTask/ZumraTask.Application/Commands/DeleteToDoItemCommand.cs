using MediatR;

namespace ZumraTask.Application.Commands;

public record DeleteToDoItemCommand(int Id) : IRequest;
