using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Company;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetMyCompany
{
    public class GetMyCompanyHandler
        : IRequestHandler<GetMyCompanyRequest, GetMyCompanyResponse>
    {
        private readonly ICompanyService _companyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyCompanyHandler(
            ICompanyService companyService,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyCompanyResponse> Handle(
            GetMyCompanyRequest request,
            CancellationToken cancellationToken)
        {
            var ownerIdClaim =
                _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value
                ??
                _httpContextAccessor.HttpContext?.User
                    .FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(ownerIdClaim))
            {
                return new GetMyCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi tapılmadı.",
                    ErrorType = ResultErrorType.Forbidden
                };
            }

            var company = await _companyService.GetMyCompanyAsync(Guid.Parse(ownerIdClaim));

            if (company == null)
            {
                return new GetMyCompanyResponse
                {
                    Succeeded = false,
                    Message = "Şirkət tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetMyCompanyResponse
            {
                Succeeded = true,
                Message = "Şirkət uğurla əldə edildi.",

                Company = new CompanyDto
                {
                    Id = company.Id,
                    Name = company.Name,
                    Description = company.Description,
                    OwnerId = company.OwnerId,
                    CreatedAt = company.CreatedAt,
                    IsDeleted = company.IsDeleted,
                    DeletedAt = company.DeletedAt
                }
            };
        }
    }
}