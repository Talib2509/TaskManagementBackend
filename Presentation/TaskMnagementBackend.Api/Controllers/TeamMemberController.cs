using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.CreateTeamMember;

using TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetAllTeamMember;
using TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetTeamMemberById;

namespace TaskMnagementBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamMemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamMemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllTeamMemberRequest());

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetTeamMemberByIdRequest
            {
                Id = id
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamMemberRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

      
    }
}