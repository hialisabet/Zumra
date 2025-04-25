using Microsoft.EntityFrameworkCore;
using Zumra.Domain.Entities;

namespace Zumra.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoItem> TodoItems { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}