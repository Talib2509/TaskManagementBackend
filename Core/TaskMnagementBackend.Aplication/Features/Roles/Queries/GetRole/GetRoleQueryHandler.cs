using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Aplication.DTOs.Role;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetRole;

public sealed class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, RoleDto?>
{
    private readonly RoleManager<AppRole> _roles;
    public GetRoleQueryHandler(RoleManager<AppRole> roles) => _roles = roles;
    public async Task<RoleDto?> Handle(GetRoleQuery request, CancellationToken cancellationToken) { var x = await _roles.FindByIdAsync(request.Id.ToString()); return x is null ? null : new RoleDto { Id = x.Id, Name = x.Name! }; }
}
