using Microsoft.EntityFrameworkCore;
using ZumraTask.Application.Interfaces;
using ZumraTask.Domain.Entities;

namespace ZumraTask.Infrastructure.Persistence.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly ApplicationDbContext _db;
    public ToDoRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() =>
        await _db.ToDoItems.Where(x => !x.IsDeleted).ToListAsync();

    public async Task<ToDoItem?> GetByIdAsync(int id) =>
        await _db.ToDoItems.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    public async Task AddAsync(ToDoItem entity)
    {
        await _db.ToDoItems.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(ToDoItem entity)
    {
        _db.ToDoItems.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;
        entity.IsDeleted = true;
        await _db.SaveChangesAsync();
    }
}
