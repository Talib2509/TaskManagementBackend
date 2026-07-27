using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyHandler
        : IRequestHandler<DeleteCompanyRequest, DeleteCompanyResponse>
    {
        private readonly ICompanyService _companyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteCompanyHandler(
            ICompanyService companyService,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeleteCompanyResponse> Handle(
            DeleteCompanyRequest request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return new DeleteCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi tapılmadı.",
                    ErrorType = ResultErrorType.Forbidden
                };
            }

            var ownerIdClaim =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                user.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(ownerIdClaim))
            {
                return new DeleteCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi Id tapılmadı.",
                    ErrorType = ResultErrorType.BadRequest
                };
            }

            if (!Guid.TryParse(ownerIdClaim, out Guid ownerId))
            {
                return new DeleteCompanyResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi Id düzgün formatda deyil.",
                    ErrorType = ResultErrorType.BadRequest
                };
            }

            if (!user.IsInRole("CompanyOwner"))
            {
                return new DeleteCompanyResponse
                {
                    Succeeded = false,
                    Message = "Bu əməliyyatı etməyə icazəniz yoxdur.",
                    ErrorType = ResultErrorType.Forbidden
                };
            }

            var company = await _companyService.GetByIdAsync(request.Id);

            if (company == null)
            {
                return new DeleteCompanyResponse
                {
                    Succeeded = false,
                    Message = "Şirkət tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            if (company.OwnerId != ownerId)
            {
                return new DeleteCompanyResponse
                {
                    Succeeded = false,
                    Message = "Bu şirkəti silməyə icazəniz yoxdur.",
                    ErrorType = ResultErrorType.Forbidden
                };
            }

            var result = await _companyService.DeleteAsync(request.Id);

            if (!result)
            {
                return new DeleteCompanyResponse
                {
                    Succeeded = false,
                    Message = "Şirkət silinərkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new DeleteCompanyResponse
            {
                Succeeded = true,
                Message = "Şirkət uğurla silindi."
            };
        }
    }
}