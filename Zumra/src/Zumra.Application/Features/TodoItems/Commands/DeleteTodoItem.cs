using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zumra.Application.Interfaces;

namespace Zumra.Application.Features.TodoItems.Commands
{
    public class DeleteTodoItem
    {
        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.TodoItems
                    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    return false;
                }

                _context.TodoItems.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}