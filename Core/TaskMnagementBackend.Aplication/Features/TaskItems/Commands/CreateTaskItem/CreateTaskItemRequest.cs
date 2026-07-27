using MediatR;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemRequest : IRequest<CreateTaskItemResponse>
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int TeamId { get; set; }

        public Guid AssignedUserId { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskItemPriority Priority { get; set; } = TaskItemPriority.Medium;
    }
}