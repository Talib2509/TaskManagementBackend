using MediatR;
using TaskMnagementBackend.Aplication.DTOs.Role;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(Guid UserId) : IRequest<UserRolesDto?>;
