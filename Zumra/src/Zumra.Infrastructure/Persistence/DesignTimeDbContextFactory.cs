using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Zumra.Infrastructure.Persistence;

public class DesignTimeDbContextFactory
  : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();
        var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseSqlServer(config.GetConnectionString("DefaultConnection"));
        return new ApplicationDbContext(opts.Options);
    }
}
