using TaskMnagementBackend.Domain.Enums;
using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public TaskType Type { get; set; }
        public TaskVisibility Visibility { get; set; } = TaskVisibility.Public;

        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
