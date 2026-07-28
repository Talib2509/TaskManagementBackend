using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.Features.Invitations.Commands.AcceptInvitation;
using TaskMnagementBackend.Aplication.Features.Invitations.Commands.DeleteTeamInvitation;
using TaskMnagementBackend.Aplication.Features.Invitations.Commands.RejectInvitation;
using TaskMnagementBackend.Aplication.Features.TeamInvitations.Commands.CreateTeamInvitation;
using TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetAllTeamInvitation;
using TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetTeamInvitationById;

namespace TaskMnagementBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamInvitationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamInvitationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllTeamInvitationRequest());

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetTeamInvitationByIdRequest
            {
                Id = id
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamInvitationRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

   

        [HttpPut("accept")]
        public async Task<IActionResult> Accept(AcceptInvitationRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("reject")]
        public async Task<IActionResult> Reject(RejectInvitationRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteTeamInvitationRequest
            {
                Id = id
            });

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}