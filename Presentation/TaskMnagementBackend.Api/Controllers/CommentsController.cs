using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Features.Commands.CreateComment;
using TaskMnagementBackend.Aplication.Features.Commands.DeleteComment;
using TaskMnagementBackend.Aplication.Features.Commands.ToggleCommentReaction;
using TaskMnagementBackend.Aplication.Features.Commands.UpdateComment;
using TaskMnagementBackend.Aplication.Features.Queries.GetTaskComments;
using TaskMnagementBackend.Aplication.Features.Queries.GetTaskTimeline;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskComments(int taskId)
        {
            var query = new GetTaskCommentsQuery(taskId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpPost]
       // [Authorize] 
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentCommand command)
        {
            
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("İstifadəci təsdiq edilmədi.");
            }

            command.UserId = userId; 

            var result = await _mediator.Send(command);

            if (result) return Ok(new { message = "Şərh uğurla əlavə edildi." });

            return BadRequest("Şərh əlavə edilərkən xəta baş verdi.");
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromForm] UpdateCommentCommand command)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized("İstifadəçi təsdiq edilmədi.");

            command.UserId = userId;
            var result = await _mediator.Send(command);

            if (result) return Ok(new { message = "Şərh redaktə olundu." });
            return BadRequest("Şərh redaktə edilərkən xəta baş verdi.");
        }



        [HttpDelete("{id}")]
       // [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized("İstifadəçi təsdiq edilmədi.");

            var command = new DeleteCommentCommand { CommentId = id, UserId = userId };
            var result = await _mediator.Send(command);

            if (result) return Ok(new { message = "Şərh silindi (Soft Delete)." });
            return BadRequest("Şərh silinərkən xəta baş verdi.");
        }


        [HttpGet("{taskId}/timeline")]
       // [Authorize]
        public async Task<IActionResult> GetTaskTimeline(int taskId)
        {
            var query = new GetTaskTimelineQuery { TaskId = taskId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("reactions")]
       // [Authorize]
        public async Task<IActionResult> ToggleReaction([FromForm] ToggleCommentReactionCommand command)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized("İstifadəçi təsdiq edilmədi.");

            command.UserId = userId;
            var result = await _mediator.Send(command);

            if (result) return Ok(new { message = "Reaksiya yeniləndi." });
            return BadRequest("Reaksiya əlavə edilərkən xəta baş verdi.");
        }
    }
}