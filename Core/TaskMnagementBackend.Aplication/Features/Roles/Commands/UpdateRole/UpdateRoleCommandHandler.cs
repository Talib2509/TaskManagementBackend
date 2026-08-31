using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResponse>
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateRoleCommandHandler(RoleManager<AppRole> roleManager, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
    { _roleManager = roleManager; _auditLogService = auditLogService; _httpContextAccessor = httpContextAccessor; }

    public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name?.Trim();
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null) return new() { Succeeded = false, ErrorType = ResultErrorType.NotFound, Message = "Role not found." };
        if (string.IsNullOrWhiteSpace(name)) return new() { Succeeded = false, ErrorType = ResultErrorType.Validation, Message = "Role name is required." };
        if (UserRoles.All.Any(x => string.Equals(x, role.Name, StringComparison.OrdinalIgnoreCase)))
            return new() { Succeeded = false, ErrorType = ResultErrorType.Forbidden, Message = "System roles cannot be renamed." };
        if (_roleManager.Roles.ToList().Any(x => x.Id != role.Id && string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)))
            return new() { Succeeded = false, ErrorType = ResultErrorType.Conflict, Message = "A role with this name already exists." };

        var oldName = role.Name;
        role.Name = name;
        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded) return new() { Succeeded = false, ErrorType = ResultErrorType.BadRequest, Message = string.Join(" ", result.Errors.Select(x => x.Description)) };
        await LogAsync(role.Id.ToString(), $"Role '{oldName}' was renamed to '{name}'.", cancellationToken);
        return new() { Succeeded = true, RoleId = role.Id, Name = role.Name, Message = "Role updated successfully." };
    }

    private Task LogAsync(string id, string details, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User; Guid.TryParse(user?.FindFirstValue(ClaimTypes.NameIdentifier), out var uid);
        return _auditLogService.LogAsync("RoleUpdated", "Role", id, details, uid == Guid.Empty ? null : uid, user?.FindFirstValue(ClaimTypes.Email), user?.FindFirstValue(ClaimTypes.Name), _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(), cancellationToken);
    }
}
