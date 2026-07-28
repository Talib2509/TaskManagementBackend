using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.Features.Notifications.Commands.CreateNotification;
using TaskMnagementBackend.Aplication.Features.Notifications.Commands.DeleteNotification;
using TaskMnagementBackend.Aplication.Features.Notifications.Commands.MarkAllNotificationsAsRead;
using TaskMnagementBackend.Aplication.Features.Notifications.Commands.MarkNotificationAsRead;
using TaskMnagementBackend.Aplication.Features.Notifications.Commands.SendNotification;
using TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetMyNotifications;
using TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationById;
using TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetNotificationCount;
using TaskMnagementBackend.Aplication.Features.Notifications.Queries.GetUnreadNotifications;

namespace TaskMnagementBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] Guid userId)
        {
            var response = await _mediator.Send(new GetNotificationByIdRequest
            {
                NotificationId = id,
                UserId = userId
            });

            return Ok(response);
        }

        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetMyNotifications(
            [FromQuery] Guid userId,
            [FromQuery] bool onlyUnread = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetMyNotificationsRequest
            {
                UserId = userId,
                OnlyUnread = onlyUnread,
                Page = page,
                PageSize = pageSize
            });

            return Ok(response);
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnread(Guid userId)
        {
            var response = await _mediator.Send(new GetUnreadNotificationsRequest
            {
                UserId = userId
            });

            return Ok(response);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount(
            [FromQuery] Guid userId,
            [FromQuery] bool onlyUnread = true)
        {
            var response = await _mediator.Send(new GetNotificationCountRequest
            {
                UserId = userId,
                OnlyUnread = onlyUnread
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(SendNotificationRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("mark-as-read")]
        public async Task<IActionResult> MarkAsRead(MarkNotificationAsReadRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead(MarkAllNotificationsAsReadRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid userId)
        {
            var response = await _mediator.Send(new DeleteNotificationRequest
            {
                NotificationId = id,
                UserId = userId
            });

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}