using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics.TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics
{
    public class GetCompanyStatisticsHandler
        : IRequestHandler<GetCompanyStatisticsRequest, GetCompanyStatisticsResponse>
    {
        private readonly ICompanyService _companyService;

        public GetCompanyStatisticsHandler(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<GetCompanyStatisticsResponse> Handle(
            GetCompanyStatisticsRequest request,
            CancellationToken cancellationToken)
        {
            var statistics = await _companyService.GetStatisticsAsync(request.CompanyId);

            if (statistics == null)
            {
                return new GetCompanyStatisticsResponse
                {
                    Succeeded = false,
                    Message = "Şirkət statistikası tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetCompanyStatisticsResponse
            {
                Succeeded = true,
                Message = "Şirkət statistikası uğurla əldə edildi.",
                Statistics = statistics
            };
        }
    }
}