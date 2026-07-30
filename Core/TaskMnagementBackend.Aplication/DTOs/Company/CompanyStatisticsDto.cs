using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.DTOs.Company
{
    public class CompanyStatisticsDto
    {
        public int TeamCount { get; set; }

        public int MemberCount { get; set; }

        public int ActiveTaskCount { get; set; }

        public int CompletedTaskCount { get; set; }

        public double CompletionRate { get; set; }
    }
}
