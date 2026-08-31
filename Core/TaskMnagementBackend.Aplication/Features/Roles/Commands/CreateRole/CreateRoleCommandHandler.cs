using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateRoleCommandHandler(RoleManager<AppRole> roleManager, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
    {
        _roleManager = roleManager;
        _auditLogService = auditLogService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return new() { Succeeded = false, ErrorType = ResultErrorType.Validation, Message = "Role name is required." };

        var exists = _roleManager.Roles.ToList().Any(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        if (exists)
            return new() { Succeeded = false, ErrorType = ResultErrorType.Conflict, Message = "A role with this name already exists." };

        var role = new AppRole { Name = name };
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
            return new() { Succeeded = false, ErrorType = ResultErrorType.BadRequest, Message = string.Join(" ", result.Errors.Select(x => x.Description)) };

        await LogAsync("RoleCreated", role.Id.ToString(), $"Role '{name}' was created.", cancellationToken);
        return new() { Succeeded = true, RoleId = role.Id, Name = role.Name, Message = "Role created successfully." };
    }

    private Task LogAsync(string action, string entityId, string details, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        Guid.TryParse(user?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        return _auditLogService.LogAsync(action, "Role", entityId, details, userId == Guid.Empty ? null : userId,
            user?.FindFirstValue(ClaimTypes.Email), user?.FindFirstValue(ClaimTypes.Name),
            _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(), cancellationToken);
    }
}
