using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Zumra.Application.Common.Behaviors;
using Zumra.Application.Features.TodoItems.Commands;

namespace Zumra.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CreateTodoItem.Command).Assembly);

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(CreateTodoItem.Handler).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        return services;
    }
}