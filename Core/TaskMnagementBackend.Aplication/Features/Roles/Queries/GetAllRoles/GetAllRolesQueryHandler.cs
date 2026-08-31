using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Aplication.DTOs.Role;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetAllRoles;

public sealed class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IReadOnlyCollection<RoleDto>>
{
    private readonly RoleManager<AppRole> _roles;
    public GetAllRolesQueryHandler(RoleManager<AppRole> roles) => _roles = roles;
    public Task<IReadOnlyCollection<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken) => Task.FromResult<IReadOnlyCollection<RoleDto>>(_roles.Roles.OrderBy(x => x.Name).Select(x => new RoleDto { Id = x.Id, Name = x.Name! }).ToList());
}
