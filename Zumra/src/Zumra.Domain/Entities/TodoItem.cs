using Zumra.Domain.Common;

namespace Zumra.Domain.Entities
{
    public class TodoItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TodoStatus Status { get; set; }
    }

    public enum TodoStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
}