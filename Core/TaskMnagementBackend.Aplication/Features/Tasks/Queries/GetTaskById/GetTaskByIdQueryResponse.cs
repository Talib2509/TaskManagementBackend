using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public TaskDto? Task { get; set; }
    }
}
