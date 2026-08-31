using MediatR;
using TaskMnagementBackend.Aplication.DTOs.Role;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetRoleUsers;

public sealed record GetRoleUsersQuery(Guid RoleId) : IRequest<IReadOnlyCollection<RoleUserDto>?>;
