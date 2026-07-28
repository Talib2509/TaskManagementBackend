using MediatR;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemRequest : IRequest<UpdateTaskItemResponse>
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int TeamId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public bool IsPrivate { get; set; }

        public TaskItemStatus Status { get; set; }
    }
}