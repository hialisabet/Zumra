using MediatR;
using Microsoft.EntityFrameworkCore;
using Zumra.Application.Interfaces;
using Zumra.Domain.Entities;

namespace Zumra.Application.Features.TodoItems.Queries
{
    public class GetAllTodoItems
    {
        public class Query : IRequest<List<TodoItemDto>> { }

        public class Handler(IApplicationDbContext context) : IRequestHandler<Query, List<TodoItemDto>>
        {
            public async Task<List<TodoItemDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.TodoItems
                    .Select(t => new TodoItemDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Status = t.Status,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }

    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TodoStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}