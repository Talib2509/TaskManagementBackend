using MediatR;

using TaskStatus = TaskMnagementBackend.Domain.Enums.TaskStatus;


namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.ChangeStatus
{
    public class ChangeTaskStatusCommandRequest : IRequest<ChangeTaskStatusCommandResponse>
    {
        public Guid TaskId { get; set; }
        public TaskStatus NewStatus { get; set; }
    }
}
