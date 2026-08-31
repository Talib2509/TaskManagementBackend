using MediatR;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.RemoveRole;

public sealed class RemoveRoleCommand : IRequest<RemoveRoleResponse>
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public sealed class RemoveRoleResponse : OperationResultBase
{
}
