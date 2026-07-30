using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Features.Commands.MarkNotificationsAsRead;
using TaskMnagementBackend.Aplication.Features.Queries.GetNotifications;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] string? type)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            var query = new GetNotificationsQuery { UserId = userId, Type = type };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("mark-as-read")]
        public async Task<IActionResult> MarkAsRead()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            var command = new MarkNotificationsAsReadCommand { UserId = userId };
            var result = await _mediator.Send(command);
            return Ok(new { message = "Bütün bildirişlər oxunmuş işarələndi." });
        }
    }
}