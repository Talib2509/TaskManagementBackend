using MediatR;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommand : IRequest<UpdateRoleResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class UpdateRoleResponse : OperationResultBase
{
    public Guid? RoleId { get; init; }
    public string? Name { get; init; }
}
