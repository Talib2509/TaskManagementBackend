using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTeamDashboard
{
    public class GetTeamDashboardQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public TeamDashboardDto Dashboard { get; set; } = new();
    }
}
