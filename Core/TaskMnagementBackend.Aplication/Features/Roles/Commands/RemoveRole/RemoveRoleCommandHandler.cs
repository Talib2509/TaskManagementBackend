using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.RemoveRole;

public sealed class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, RemoveRoleResponse>
{
    private readonly UserManager<AppUser> _users; private readonly RoleManager<AppRole> _roles; private readonly IAuditLogService _audit; private readonly IHttpContextAccessor _http;
    public RemoveRoleCommandHandler(UserManager<AppUser> users, RoleManager<AppRole> roles, IAuditLogService audit, IHttpContextAccessor http) { _users = users; _roles = roles; _audit = audit; _http = http; }
    public async Task<RemoveRoleResponse> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var name = request.RoleName?.Trim(); if (string.IsNullOrWhiteSpace(name)) return new() { ErrorType = ResultErrorType.Validation, Message = "Role name is required." };
        var user = await _users.FindByIdAsync(request.UserId.ToString()); if (user is null) return new() { ErrorType = ResultErrorType.NotFound, Message = "User not found." };
        var role = await _roles.FindByNameAsync(name); if (role is null) return new() { ErrorType = ResultErrorType.NotFound, Message = "Role not found." };
        if (UserRoles.All.Any(x => string.Equals(x, role.Name, StringComparison.OrdinalIgnoreCase))) return new() { ErrorType = ResultErrorType.Forbidden, Message = "System roles cannot be removed." };
        if (!await _users.IsInRoleAsync(user, role.Name!)) return new() { ErrorType = ResultErrorType.BadRequest, Message = "User does not have this role." };
        var result = await _users.RemoveFromRoleAsync(user, role.Name!); if (!result.Succeeded) return new() { ErrorType = ResultErrorType.BadRequest, Message = string.Join(" ", result.Errors.Select(x => x.Description)) };
        await AuditAsync(role.Id.ToString(), $"Role '{role.Name}' removed from user '{user.Email}'.", cancellationToken);
        return new() { Succeeded = true, Message = "Role removed successfully." };
    }
    private Task AuditAsync(string id, string details, CancellationToken ct) { var u = _http.HttpContext?.User; Guid.TryParse(u?.FindFirstValue(ClaimTypes.NameIdentifier), out var uid); return _audit.LogAsync("RoleRemoved", "Role", id, details, uid == Guid.Empty ? null : uid, u?.FindFirstValue(ClaimTypes.Email), u?.FindFirstValue(ClaimTypes.Name), _http.HttpContext?.Connection.RemoteIpAddress?.ToString(), ct); }
}
