using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, DeleteRoleResponse>
{
    private readonly RoleManager<AppRole> _roleManager; private readonly UserManager<AppUser> _userManager; private readonly IAuditLogService _audit; private readonly IHttpContextAccessor _http;
    public DeleteRoleCommandHandler(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IAuditLogService audit, IHttpContextAccessor http) { _roleManager = roleManager; _userManager = userManager; _audit = audit; _http = http; }
    public async Task<DeleteRoleResponse> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null) return new() { ErrorType = ResultErrorType.NotFound, Message = "Role not found." };
        if (UserRoles.All.Any(x => string.Equals(x, role.Name, StringComparison.OrdinalIgnoreCase))) return new() { ErrorType = ResultErrorType.Forbidden, Message = "System roles cannot be deleted." };
        var users = await _userManager.GetUsersInRoleAsync(role.Name!);
        if (users.Count > 0) return new() { ErrorType = ResultErrorType.Conflict, Message = "Role is assigned to users and cannot be deleted." };
        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded) return new() { ErrorType = ResultErrorType.BadRequest, Message = string.Join(" ", result.Errors.Select(x => x.Description)) };
        await AuditAsync(role.Id.ToString(), $"Role '{role.Name}' was deleted.", cancellationToken);
        return new() { Succeeded = true, Message = "Role deleted successfully." };
    }
    private Task AuditAsync(string id, string details, CancellationToken ct) { var u = _http.HttpContext?.User; Guid.TryParse(u?.FindFirstValue(ClaimTypes.NameIdentifier), out var uid); return _audit.LogAsync("RoleDeleted", "Role", id, details, uid == Guid.Empty ? null : uid, u?.FindFirstValue(ClaimTypes.Email), u?.FindFirstValue(ClaimTypes.Name), _http.HttpContext?.Connection.RemoteIpAddress?.ToString(), ct); }
}
