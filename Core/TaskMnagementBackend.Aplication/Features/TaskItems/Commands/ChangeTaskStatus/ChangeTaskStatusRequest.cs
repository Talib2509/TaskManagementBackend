using MediatR;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.ChangeTaskStatus
{
    public class ChangeTaskStatusRequest : IRequest<ChangeTaskStatusResponse>
    {
        public int Id { get; set; }

        public TaskItemStatus Status { get; set; }
    }
}