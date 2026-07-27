using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.ChangeStatus;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.ClaimTask;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.CreateTask;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.CreateTeamTask;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.DeleteTask;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.ReassignTask;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.Subtasks;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.ToggleSubTask;
using TaskMnagementBackend.Aplication.Features.Tasks.Commands.UpdateTask;
using TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetKanbanBoard;
using TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetMyTeamTasks;
using TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTaskById;
using TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTasksList;
using TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTeamDashboard;
using TaskMnagementBackend.Aplication.Features.Tasks.Queries.SearchTasks;


namespace TaskMnagementBackend.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommandRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskCommandRequest request)
        {
            request.Id = id;

            var response = await _mediator.Send(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteTaskCommandRequest { Id = id });

            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetTaskByIdQueryRequest { Id = id });
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetTasksListQueryRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusDto dto)
        {
            var response = await _mediator.Send(new ChangeTaskStatusCommandRequest 
            { 
                TaskId = id,
                NewStatus = dto.NewStatus,
            });

            if (!response.Success)
                return BadRequest(response);
            
            return Ok(response);
        }

        [HttpPost("{taskId:guid}/subtasks")]
        public async Task<IActionResult> CreateSubTask(Guid taskId, [FromBody] CreateSubTaskDto dto)
        {
            var response = await _mediator.Send(new CreateSubTaskCommandRequest
            {
                TaskId = taskId,
                Text = dto.Text
            });

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // ☑️ Переключение статуса subtask (isCompleted toggle)
        // PATCH: /api/tasks/subtasks/{subTaskId}/toggle
        [HttpPatch("subtasks/{subTaskId:guid}/toggle")]
        public async Task<IActionResult> ToggleSubTask(Guid subTaskId)
        {
            var response = await _mediator.Send(new ToggleSubTaskCommandRequest { SubTaskId = subTaskId });

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("board")]
        public async Task<IActionResult> GetKanbanBoGetBoardard()
        {
            var response = await _mediator.Send(new GetKanbanBoardQueryRequest());

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // 🔍 Поиск по названию и описанию
        // GET: /api/tasks/search?q=ноутбук
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var response = await _mediator.Send(new SearchTasksQueryRequest { Query = q });

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }



        // 👥 Создание командной задачи (Team Lead / Owner)
        // POST: /api/tasks/team
        [HttpPost("team")]
        public async Task<IActionResult> CreateTeamTask([FromBody] CreateTeamTaskCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        // ✋ Забрать публичную задачу себе (Claim Task)
        // POST: /api/tasks/{id}/claim
        [HttpPost("{id:guid}/claim")]
        public async Task<IActionResult> ClaimTask(Guid id)
        {
            var response = await _mediator.Send(new ClaimTaskCommandRequest { TaskId = id });
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        // 🔄 Переназначить задачу на другого исполнителя
        // POST: /api/tasks/reassign
        [HttpPost("reassign")]
        public async Task<IActionResult> ReassignTask([FromBody] ReassignTaskCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        // 📋 Мои командные задачи (Отсортированные по Priority & Deadline)
        // GET: /api/tasks/my-team-tasks
        [HttpGet("my-team-tasks")]
        public async Task<IActionResult> GetMyTeamTasks()
        {
            var response = await _mediator.Send(new GetMyTeamTasksQueryRequest());
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        // 📊 Dashboard для Team Lead
        // GET: /api/tasks/team-dashboard?teamId=...
        [HttpGet("team-dashboard")]
        public async Task<IActionResult> GetTeamDashboard([FromQuery] Guid teamId)
        {
            var response = await _mediator.Send(new GetTeamDashboardQueryRequest { TeamId = teamId });
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
