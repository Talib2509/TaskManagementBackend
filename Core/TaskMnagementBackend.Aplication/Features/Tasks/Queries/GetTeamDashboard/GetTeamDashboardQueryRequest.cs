using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTeamDashboard
{
    public class GetTeamDashboardQueryRequest : IRequest<GetTeamDashboardQueryResponse>
    {
        public Guid TeamId { get; set; }
    }
}
