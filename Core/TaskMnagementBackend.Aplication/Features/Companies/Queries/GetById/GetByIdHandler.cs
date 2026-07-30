using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly ICompanyService _companyService;

        public GetByIdHandler(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                return new GetByIdResponse
                {
                    Succeeded = false,
                    ErrorType = ResultErrorType.Validation,
                    Message = "Şirkət ID düzgün deyil."
                };
            }

            var company = await _companyService.GetByIdAsync(request.Id);

            if (company is null)
            {
                return new GetByIdResponse
                {
                    Succeeded = false,
                    ErrorType = ResultErrorType.NotFound,
                    Message = "Şirkət tapılmadı."
                };
            }

            return new GetByIdResponse
            {
                Succeeded = true,
                Message = "Şirkət tapıldı.",
                Company = company
            };
        }
    }
}
