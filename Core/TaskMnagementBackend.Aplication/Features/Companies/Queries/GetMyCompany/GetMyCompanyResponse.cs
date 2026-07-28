using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Company;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetMyCompany
{
    public class GetMyCompanyResponse : OperationResultBase
    {
        public CompanyDto? Company { get; set; }
    }
}