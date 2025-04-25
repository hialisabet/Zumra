using ZumraTask.Domain.Enums;

namespace ZumraTask.Domain.Entities;

public class ToDoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ToDoStatus Status { get; set; }
}