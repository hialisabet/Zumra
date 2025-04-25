using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Zumra.Application.Common.Behaviors;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Zumra.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}