using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zumra.Application.Interfaces;

namespace Zumra.Application.Features.TodoItems.Queries
{
    public class GetTodoItemById
    {
        public class Query : IRequest<TodoItemDto?>
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler(IApplicationDbContext context) : IRequestHandler<Query, TodoItemDto?>
        {
            public async Task<TodoItemDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.TodoItems
                    .Where(t => t.Id == request.Id)
                    .Select(t => new TodoItemDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Status = t.Status,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt
                    })
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}