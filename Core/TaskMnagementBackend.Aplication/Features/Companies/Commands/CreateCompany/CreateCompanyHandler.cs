using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.Company;
using TaskMnagementBackend.Aplication.Features.Companies.Commands.CreateCompany;

namespace TaskMnagementBackend.Aplication.Features.Company.Commands.CreateCompany
{
    public class CreateCompanyCommandHandler
        : IRequestHandler<CreateCompanyRequest, CreateCompanyResponse>
    {
        private readonly ICompanyService _companyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCompanyCommandHandler(
            ICompanyService companyService,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateCompanyResponse> Handle(
            CreateCompanyRequest request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return new CreateCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi tapılmadı."
                };
            }

            var ownerIdClaim =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                user.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(ownerIdClaim))
            {
                return new CreateCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi Id tapılmadı."
                };
            }

            if (!Guid.TryParse(ownerIdClaim, out Guid ownerId))
            {
                return new CreateCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi Id düzgün formatda deyil."
                };
            }

            if (!user.IsInRole("CompanyOwner"))
            {
                return new CreateCompanyResponse
                {
                    Succeeded = false,
                    Message = "Yalnız CompanyOwner şirkət yarada bilər."
                };
            }

            var dto = new CreateCompanyDto
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = ownerId
            };

            var result = await _companyService.CreateAsync(dto);

            return new CreateCompanyResponse
            {
                Succeeded = result,
                Message = result
                    ? "Şirkət uğurla yaradıldı."
                    : "Şirkət yaradılarkən xəta baş verdi."
            };
        }
    }
}