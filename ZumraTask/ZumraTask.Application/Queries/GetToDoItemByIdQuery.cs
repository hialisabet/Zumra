using MediatR;
using ZumraTask.Application.Dtos;

namespace ZumraTask.Application.Queries;

public record GetToDoItemByIdQuery(int Id) : IRequest<ToDoItemDto>;
