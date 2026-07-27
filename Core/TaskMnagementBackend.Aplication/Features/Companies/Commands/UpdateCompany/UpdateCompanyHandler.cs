using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.Company;
using TaskMnagementBackend.Aplication.Features.Companies.Commands.UpdateCompany;

namespace TaskMnagementBackend.Aplication.Features.Company.Commands.UpdateCompany
{
    public class UpdateCompanyHandler
        : IRequestHandler<UpdateCompanyRequest, UpdateCompanyResponse>
    {
        private readonly ICompanyService _companyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCompanyHandler(
            ICompanyService companyService,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateCompanyResponse> Handle(
            UpdateCompanyRequest request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return new UpdateCompanyResponse
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
                return new UpdateCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi Id tapılmadı."
                };
            }

            if (!Guid.TryParse(ownerIdClaim, out Guid ownerId))
            {
                return new UpdateCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi Id düzgün formatda deyil."
                };
            }

            if (!user.IsInRole("CompanyOwner"))
            {
                return new UpdateCompanyResponse
                {
                    Succeeded = false,
                    Message = "Yalnız CompanyOwner şirkəti yeniləyə bilər."
                };
            }

            var company = await _companyService.GetByIdAsync(request.Id);

            if (company == null)
            {
                return new UpdateCompanyResponse
                {
                    Succeeded = false,
                    Message = "Şirkət tapılmadı."
                };
            }

            if (company.OwnerId != ownerId)
            {
                return new UpdateCompanyResponse
                {
                    Succeeded = false,
                    Message = "Bu şirkəti yeniləməyə icazəniz yoxdur."
                };
            }

            var dto = new UpdateCompanyDto
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };

            var result = await _companyService.UpdateAsync(dto);

            return new UpdateCompanyResponse
            {
                Succeeded = result,
                Message = result
                    ? "Şirkət uğurla yeniləndi."
                    : "Şirkət yenilənərkən xəta baş verdi."
            };
        }
    }
}