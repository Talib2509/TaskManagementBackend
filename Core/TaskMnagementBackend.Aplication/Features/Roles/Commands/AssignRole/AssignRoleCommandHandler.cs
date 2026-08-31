using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.AssignRole;

public sealed class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, AssignRoleResponse>
{
    private readonly UserManager<AppUser> _users; private readonly RoleManager<AppRole> _roles; private readonly IAuditLogService _audit; private readonly IHttpContextAccessor _http;
    public AssignRoleCommandHandler(UserManager<AppUser> users, RoleManager<AppRole> roles, IAuditLogService audit, IHttpContextAccessor http) { _users = users; _roles = roles; _audit = audit; _http = http; }
    public async Task<AssignRoleResponse> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var name = request.RoleName?.Trim(); if (string.IsNullOrWhiteSpace(name)) return new() { ErrorType = ResultErrorType.Validation, Message = "Role name is required." };
        var user = await _users.FindByIdAsync(request.UserId.ToString()); if (user is null) return new() { ErrorType = ResultErrorType.NotFound, Message = "User not found." };
        var role = await _roles.FindByNameAsync(name); if (role is null) return new() { ErrorType = ResultErrorType.NotFound, Message = "Role not found." };
        if (await _users.IsInRoleAsync(user, role.Name!)) return new() { ErrorType = ResultErrorType.Conflict, Message = "User already has this role." };
        var result = await _users.AddToRoleAsync(user, role.Name!); if (!result.Succeeded) return new() { ErrorType = ResultErrorType.BadRequest, Message = string.Join(" ", result.Errors.Select(x => x.Description)) };
        await AuditAsync(role.Id.ToString(), $"Role '{role.Name}' assigned to user '{user.Email}'.", cancellationToken);
        return new() { Succeeded = true, Message = "Role assigned successfully." };
    }
    private Task AuditAsync(string id, string details, CancellationToken ct) { var u = _http.HttpContext?.User; Guid.TryParse(u?.FindFirstValue(ClaimTypes.NameIdentifier), out var uid); return _audit.LogAsync("RoleAssigned", "Role", id, details, uid == Guid.Empty ? null : uid, u?.FindFirstValue(ClaimTypes.Email), u?.FindFirstValue(ClaimTypes.Name), _http.HttpContext?.Connection.RemoteIpAddress?.ToString(), ct); }
}
