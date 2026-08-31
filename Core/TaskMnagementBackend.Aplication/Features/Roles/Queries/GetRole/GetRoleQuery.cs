using MediatR;
using TaskMnagementBackend.Aplication.DTOs.Role;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetRole;

public sealed record GetRoleQuery(Guid Id) : IRequest<RoleDto?>;
