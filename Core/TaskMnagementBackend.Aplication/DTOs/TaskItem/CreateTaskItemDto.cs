using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs.TaskItem
{
    public class CreateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int TeamId { get; set; }

        public Guid AssignedUserId { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskItemPriority Priority { get; set; }
        public bool IsPrivate { get; set; }
    }
}