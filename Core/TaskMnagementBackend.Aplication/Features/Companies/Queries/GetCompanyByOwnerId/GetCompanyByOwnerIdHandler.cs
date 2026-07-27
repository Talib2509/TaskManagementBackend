using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Company;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyByOwnerId
{
    public class GetCompanyByOwnerIdHandler
        : IRequestHandler<GetCompanyByOwnerIdRequest, GetCompanyByOwnerIdResponse>
    {
        private readonly ICompanyService _companyService;

        public GetCompanyByOwnerIdHandler(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<GetCompanyByOwnerIdResponse> Handle(
            GetCompanyByOwnerIdRequest request,
            CancellationToken cancellationToken)
        {
            var company = await _companyService.GetByOwnerIdAsync(request.OwnerId);

            if (company == null)
            {
                return new GetCompanyByOwnerIdResponse
                {
                    Succeeded = false,
                    Message = "Şirkət tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetCompanyByOwnerIdResponse
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