using MediatR;
using ZumraTask.Application.Commands;
using ZumraTask.Application.Dtos;
using ZumraTask.Application.Interfaces;
using ZumraTask.Domain.Entities;
using ZumraTask.Domain.Enums;

namespace ZumraTask.Application.Handlers;

public class CreateToDoItemCommandHandler
    : IRequestHandler<CreateToDoItemCommand, ToDoItemDto>
{
    private readonly IToDoRepository _repo;
    public CreateToDoItemCommandHandler(IToDoRepository repo) =>
        _repo = repo;

    public async Task<ToDoItemDto> Handle(
        CreateToDoItemCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new ToDoItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = ToDoStatus.Pending
        };

        await _repo.AddAsync(entity);

        return new ToDoItemDto(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Status.ToString());
    }
}
