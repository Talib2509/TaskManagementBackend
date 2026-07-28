using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Company;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyByOwnerId
{
    public class GetCompanyByOwnerIdResponse : OperationResultBase
    {
        public CompanyDto? Company { get; set; }
    }
}