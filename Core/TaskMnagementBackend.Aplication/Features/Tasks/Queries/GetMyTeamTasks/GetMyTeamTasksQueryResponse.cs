using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetMyTeamTasks
{
    public class GetMyTeamTasksQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<TaskDto> Tasks { get; set; } = new();
    }
}
