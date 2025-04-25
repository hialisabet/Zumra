using MediatR;
using System.Collections.Generic;
using ZumraTask.Application.Dtos;

namespace ZumraTask.Application.Queries;

public record GetAllToDoItemsQuery() : IRequest<IEnumerable<ToDoItemDto>>;
