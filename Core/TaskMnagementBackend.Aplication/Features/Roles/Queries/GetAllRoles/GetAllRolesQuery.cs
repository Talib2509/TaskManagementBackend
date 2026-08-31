using MediatR;
using TaskMnagementBackend.Aplication.DTOs.Role;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetAllRoles;

public sealed record GetAllRolesQuery : IRequest<IReadOnlyCollection<RoleDto>>;
