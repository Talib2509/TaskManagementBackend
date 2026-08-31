using MediatR;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.AssignRole;

public sealed class AssignRoleCommand : IRequest<AssignRoleResponse>
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public sealed class AssignRoleResponse : OperationResultBase
{
}
