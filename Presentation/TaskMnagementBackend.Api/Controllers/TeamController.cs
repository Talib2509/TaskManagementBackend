using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.Features.Teams.Commands.AssignTeamLead;
using TaskMnagementBackend.Aplication.Features.Teams.Commands.CreateTeam;
using TaskMnagementBackend.Aplication.Features.Teams.Commands.DeleteTeam;
using TaskMnagementBackend.Aplication.Features.Teams.Commands.UpdateTeam;
using TaskMnagementBackend.Aplication.Features.Teams.Queries.GetAllTeam;
using TaskMnagementBackend.Aplication.Features.Teams.Queries.GetMyTeams;
using TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamById;
using TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamStatistics;

namespace TaskMnagementBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllTeamRequest());

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetTeamByIdRequest
            {
                Id = id
            });

            return Ok(response);
        }

        [HttpGet("my-teams")]
        public async Task<IActionResult> GetMyTeams([FromQuery] Guid userId)
        {
            var response = await _mediator.Send(new GetMyTeamsRequest
            {
                UserId = userId
            });

            return Ok(response);
        }

        [HttpGet("{id}/statistics")]
        public async Task<IActionResult> GetStatistics(int id)
        {
            var response = await _mediator.Send(new GetTeamStatisticsRequest
            {
                TeamId = id
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTeamRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("assign-lead")]
        public async Task<IActionResult> AssignLead(AssignTeamLeadRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteTeamRequest
            {
                TeamId = id
            });

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}