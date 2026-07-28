using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMnagementBackend.Aplication.Features.Companies.Commands.CreateCompany;
using TaskMnagementBackend.Aplication.Features.Companies.Commands.DeleteCompany;
using TaskMnagementBackend.Aplication.Features.Companies.Commands.UpdateCompany;
using TaskMnagementBackend.Aplication.Features.Companies.Queries.GetAllCompany;
using TaskMnagementBackend.Aplication.Features.Companies.Queries.GetById;

using TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyByOwnerId;
using TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics;
using TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics.TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics;
using TaskMnagementBackend.Aplication.Features.Companies.Queries.GetMyCompany;

namespace TaskMnagementBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllCompanyRequest());

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetByIdRequest
            {
                Id = id
               
            });

            return Ok(response);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<IActionResult> GetByOwnerId(Guid ownerId)
        {
            var response = await _mediator.Send(new GetCompanyByOwnerIdRequest
            {
                OwnerId = ownerId
            });

            return Ok(response);
        }

        [HttpGet("my-company")]
        public async Task<IActionResult> GetMyCompany()
        {
            var response = await _mediator.Send(new GetMyCompanyRequest());

            return Ok(response);
        }

        [HttpGet("{id}/statistics")]
        public async Task<IActionResult> GetStatistics(int id)
        {
            var response = await _mediator.Send(new GetCompanyStatisticsRequest
            {
                CompanyId = id
            });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCompanyRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteCompanyRequest
            {
                Id = id
            });

            if (!response.Succeeded)
                return BadRequest(response);

            return Ok(response);
        }
    }
}