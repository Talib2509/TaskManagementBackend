using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class TeamDashboardDto
    {
        public int PendingCount { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int OverdueCount { get; set; }
        public List<MemberWorkloadDto> Workload { get; set; } = new();
    }
}
