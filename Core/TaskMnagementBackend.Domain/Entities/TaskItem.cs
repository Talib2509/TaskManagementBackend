using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Domain.Entities
{
   
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public int TeamId { get; set; }

        public Team? Team { get; set; }

        public Guid? AssignedUserId { get; set; }

        public AppUser? AssignedUser { get; set; }

       
        public bool IsPrivate { get; set; } = false;
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskItemPriority Priority { get; set; } = TaskItemPriority.Medium;

        public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }
    }
}
