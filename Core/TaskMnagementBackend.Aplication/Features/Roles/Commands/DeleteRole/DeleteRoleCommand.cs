using MediatR;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommand : IRequest<DeleteRoleResponse>
{
    public Guid Id { get; set; }
}

public sealed class DeleteRoleResponse : OperationResultBase
{
}
