using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.DTOs.TaskItem
{
    public class UpdateTaskItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int TeamId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public bool IsPrivate { get; set; }

        public TaskItemStatus Status { get; set; }
    }
}