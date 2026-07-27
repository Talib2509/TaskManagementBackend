using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Company;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics
{
    public class GetCompanyStatisticsResponse : OperationResultBase
    {
        public CompanyStatisticsDto? Statistics { get; set; }
    }
}