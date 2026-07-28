using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics
{
    using MediatR;

    namespace TaskMnagementBackend.Aplication.Features.Companies.Queries.GetCompanyStatistics
    {
        public class GetCompanyStatisticsRequest : IRequest<GetCompanyStatisticsResponse>
        {
            public int CompanyId { get; set; }
        }
    }
}
