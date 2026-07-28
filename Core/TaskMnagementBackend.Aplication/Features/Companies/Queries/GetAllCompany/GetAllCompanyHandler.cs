using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.Company;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetAllCompany
{
    public class GetAllCompanyHandler
        : IRequestHandler<GetAllCompanyRequest, GetAllCompanyResponse>
    {
        private readonly ICompanyService _companyService;

        public GetAllCompanyHandler(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<GetAllCompanyResponse> Handle(
            GetAllCompanyRequest request,
            CancellationToken cancellationToken)
        {
            var query = _companyService.GetAll();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(x =>
                    x.Name.ToLower().Contains(search) ||
                    (x.Description != null &&
                     x.Description.ToLower().Contains(search)));
            }

            var sortMap = new Dictionary<string, System.Linq.Expressions.Expression<Func<CompanyDto, object>>>
            {
                ["id"] = x => x.Id,
                ["name"] = x => x.Name,
                ["createdat"] = x => x.CreatedAt
            };

            query = query.ApplySort(
                request.SortBy,
                request.Desc,
                sortMap,
                "createdat");

            var result = await query.ToPagedResultAsync(
                request.Page,
                request.PageSize,
                cancellationToken);

            return new GetAllCompanyResponse
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
    }
}