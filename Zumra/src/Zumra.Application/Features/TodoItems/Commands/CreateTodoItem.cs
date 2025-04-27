using FluentValidation;
using MediatR;
using Zumra.Application.Common;
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
                RuleFor(x => x.Title)
                  .NotEmpty()
                  .MaximumLength(AppConstants.TitleMaxLength);

                RuleFor(x => x.Description)
                  .MaximumLength(AppConstants.DescriptionMaxLength);
            }
        }

        public class Handler(IApplicationDbContext context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = new TodoItem
                {
                    Title = request.Title,
                    Description = request.Description,
                    Status = TodoStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                context.TodoItems.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}