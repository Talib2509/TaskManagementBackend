using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.AssignTaskMember
{
    public class AssignTaskMemberRequest : IRequest<AssignTaskMemberResponse>
    {
        public int TaskId { get; set; }

        public Guid UserId { get; set; }
    }
}