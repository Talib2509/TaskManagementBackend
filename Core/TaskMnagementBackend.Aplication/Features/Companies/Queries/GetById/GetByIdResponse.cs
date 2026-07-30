using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.Company;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetById
{
    public class GetByIdResponse : OperationResultBase
    {
        public CompanyDto? Company { get; set; }
    }
}
