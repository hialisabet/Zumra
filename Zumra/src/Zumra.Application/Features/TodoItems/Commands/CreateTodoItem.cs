using FluentValidation;
using MediatR;
using Zumra.Application.Interfaces;
using Zumra.Domain.Entities;

namespace Zumra.Application.Features.TodoItems.Commands
{
    public class CreateTodoItem
    {
        public class Command : IRequest<int>
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
                RuleFor(x => x.Description).MaximumLength(1000);
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new TodoItem
                {
                    Title = request.Title,
                    Description = request.Description,
                    Status = TodoStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.TodoItems.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}