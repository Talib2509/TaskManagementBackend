using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Aplication.DTOs.Role;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetRoleUsers;

public sealed class GetRoleUsersQueryHandler : IRequestHandler<GetRoleUsersQuery, IReadOnlyCollection<RoleUserDto>?>
{
    private readonly RoleManager<AppRole> _roles; private readonly UserManager<AppUser> _users;
    public GetRoleUsersQueryHandler(RoleManager<AppRole> roles, UserManager<AppUser> users) { _roles = roles; _users = users; }
    public async Task<IReadOnlyCollection<RoleUserDto>?> Handle(GetRoleUsersQuery request, CancellationToken cancellationToken)
    {
        var role = await _roles.FindByIdAsync(request.RoleId.ToString()); if (role is null) return null;
        var users = await _users.GetUsersInRoleAsync(role.Name!);
        return users.Select(x => new RoleUserDto { Id = x.Id, UserName = x.UserName, Email = x.Email, FullName = x.FullName }).ToList();
    }
}
