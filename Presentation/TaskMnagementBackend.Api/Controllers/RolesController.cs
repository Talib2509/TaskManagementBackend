using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Features.Roles.Commands.AssignRole;
using TaskMnagementBackend.Aplication.Features.Roles.Commands.CreateRole;
using TaskMnagementBackend.Aplication.Features.Roles.Commands.DeleteRole;
using TaskMnagementBackend.Aplication.Features.Roles.Commands.RemoveRole;
using TaskMnagementBackend.Aplication.Features.Roles.Commands.UpdateRole;
using TaskMnagementBackend.Aplication.Features.Roles.Queries.GetAllRoles;
using TaskMnagementBackend.Aplication.Features.Roles.Queries.GetRole;
using TaskMnagementBackend.Aplication.Features.Roles.Queries.GetRoleUsers;
using TaskMnagementBackend.Aplication.Features.Roles.Queries.GetUserRoles;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.SuperAdmin}")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            return Ok(roles);
        }

        [HttpGet("all")]
        public Task<IActionResult> GetAllRolesLegacy() => GetAllRoles();

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var role = await _mediator.Send(new GetRoleQuery(id));
            return role is null ? NotFound(new { message = "Role not found." }) : Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpPost("users/{userId:guid}/assign")]
        public async Task<IActionResult> AssignRole(Guid userId, [FromBody] AssignRoleCommand command)
        {
            command.UserId = userId;
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await _mediator.Send(new DeleteRoleCommand { Id = id });
            return ToActionResult(result);
        }

        [HttpDelete("users/{userId:guid}/{roleName}")]
        public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
        {
            var result = await _mediator.Send(new RemoveRoleCommand { UserId = userId, RoleName = roleName });
            return ToActionResult(result);
        }

        [HttpGet("users/{userId:guid}")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var result = await _mediator.Send(new GetUserRolesQuery(userId));
            return result is null ? NotFound(new { message = "User not found." }) : Ok(result);
        }

        [HttpGet("{roleId:guid}/users")]
        public async Task<IActionResult> GetRoleUsers(Guid roleId)
        {
            var result = await _mediator.Send(new GetRoleUsersQuery(roleId));
            return result is null ? NotFound(new { message = "Role not found." }) : Ok(result);
        }

        private IActionResult ToActionResult(TaskMnagementBackend.Aplication.Common.OperationResultBase result)
        {
            if (result.Succeeded) return Ok(result);
            return result.ErrorType switch
            {
                TaskMnagementBackend.Aplication.Common.ResultErrorType.NotFound => NotFound(new { message = result.Message }),
                TaskMnagementBackend.Aplication.Common.ResultErrorType.Conflict => Conflict(new { message = result.Message }),
                TaskMnagementBackend.Aplication.Common.ResultErrorType.Forbidden => Forbid(),
                _ => BadRequest(new { message = result.Message })
            };
        }
    }
}