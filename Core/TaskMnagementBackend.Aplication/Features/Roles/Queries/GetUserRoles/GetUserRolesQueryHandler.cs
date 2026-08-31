using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Aplication.DTOs.Role;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Roles.Queries.GetUserRoles;

public sealed class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, UserRolesDto?>
{
    private readonly UserManager<AppUser> _users;
    public GetUserRolesQueryHandler(UserManager<AppUser> users) => _users = users;
    public async Task<UserRolesDto?> Handle(GetUserRolesQuery request, CancellationToken cancellationToken) { var user = await _users.FindByIdAsync(request.UserId.ToString()); if (user is null) return null; return new UserRolesDto { UserId = user.Id, Roles = (await _users.GetRolesAsync(user)).ToList() }; }
}
