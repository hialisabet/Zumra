using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zumra.Application.Interfaces;
using Zumra.Domain.Entities;

namespace Zumra.Application.Features.TodoItems.Commands
{
    public class UpdateTodoItem
    {
        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public TodoStatus Status { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
                RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
                RuleFor(x => x.Description).MaximumLength(1000);
            }
        }

        public class Handler(IApplicationDbContext context) : IRequestHandler<Command, bool>
        {
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await context.TodoItems
                    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                    return false;

                entity.Title = request.Title;
                entity.Description = request.Description;
                entity.Status = request.Status;
                entity.UpdatedAt = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}