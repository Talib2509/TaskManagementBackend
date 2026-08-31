using MediatR;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommand : IRequest<CreateRoleResponse>
{
    public string Name { get; set; } = string.Empty;
}

public sealed class CreateRoleResponse : OperationResultBase
{
    public Guid? RoleId { get; init; }
    public string? Name { get; init; }
}
