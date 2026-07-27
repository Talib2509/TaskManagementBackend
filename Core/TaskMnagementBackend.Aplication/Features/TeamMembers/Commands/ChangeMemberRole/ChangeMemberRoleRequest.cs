using MediatR;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.ChangeMemberRole
{
    public class ChangeMemberRoleRequest : IRequest<ChangeMemberRoleResponse>
    {
        public int TeamId { get; set; }

        public Guid UserId { get; set; }

        public TeamMemberRole Role { get; set; }
    }
}