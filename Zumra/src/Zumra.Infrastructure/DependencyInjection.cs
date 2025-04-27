using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zumra.Application.Interfaces;
using Zumra.Infrastructure.Constants;
using Zumra.Infrastructure.Persistence;

namespace Zumra.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection s, IConfiguration cfg)
    {
        s.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseSqlServer(
                cfg.GetConnectionString("DefaultConnection"),
                sqlOpts => sqlOpts
                  .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                  .MigrationsHistoryTable(
                     EfConstants.MigrationsHistoryTable,
                     EfConstants.MigrationsSchema)
            ));

        s.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        return s;
    }
}