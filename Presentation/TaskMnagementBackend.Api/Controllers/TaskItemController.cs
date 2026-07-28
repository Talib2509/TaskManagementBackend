using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.Features.TaskItems.Commands.ChangeTaskStatus;
using TaskMnagementBackend.Aplication.Features.TaskItems.Commands.CreateTaskItem;
using TaskMnagementBackend.Aplication.Features.TaskItems.Commands.DeleteTaskItem;
using TaskMnagementBackend.Aplication.Features.TaskItems.Commands.UpdateTaskItem;
using TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetAllTaskItem;
using TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetMyTasks;
using TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTaskItemById;
using TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTasksByTeam;

namespace TaskMnagementBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllTaskItemRequest());

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetTaskItemByIdRequest
            {
                Id = id
            });

            return Ok(response);
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetTasksByTeam(int teamId)
        {
            var response = await _mediator.Send(new GetTasksByTeamRequest
            {
                TeamId = teamId
            });

            return Ok(response);
        }

        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks([FromQuery] Guid userId)
        {
            var response = await _mediator.Send(new GetMyTasksRequest
            {
                UserId = userId
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskItemRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskItemRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeStatus(ChangeTaskStatusRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteTaskItemRequest
            {
                Id = id
            });

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}