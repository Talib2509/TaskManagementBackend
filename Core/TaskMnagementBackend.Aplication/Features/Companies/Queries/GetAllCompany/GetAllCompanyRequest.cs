using MediatR;
using TaskMnagementBackend.Aplication.Common.Pagination;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetAllCompany
{
  
    public class GetAllCompanyRequest : PagedRequest, IRequest<GetAllCompanyResponse>
    {
       
        public string? Search { get; set; }
    }
}
