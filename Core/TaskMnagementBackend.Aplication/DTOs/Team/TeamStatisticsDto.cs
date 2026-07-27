using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.DTOs.Team
{
    public class TeamStatisticsDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; } = string.Empty;

        public int MemberCount { get; set; }

        public int ActiveTaskCount { get; set; }

        public int CompletedTaskCount { get; set; }

        public double CompletionRate { get; set; }
    }
}
