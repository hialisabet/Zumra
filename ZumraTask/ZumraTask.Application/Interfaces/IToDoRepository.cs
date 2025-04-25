using ZumraTask.Domain.Entities;

namespace ZumraTask.Application.Interfaces;

public interface IToDoRepository
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem?> GetByIdAsync(int id);
    Task AddAsync(ToDoItem entity);
    Task UpdateAsync(ToDoItem entity);
    Task DeleteAsync(int id);
}