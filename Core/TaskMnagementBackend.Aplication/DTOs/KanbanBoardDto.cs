using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class KanbanBoardDto
    {
        public List<TaskDto> Pending { get; set; } = new();
        public List<TaskDto> InProgress { get; set; } = new();
        public List<TaskDto> Completed { get; set; } = new();
        public List<TaskDto> Blocked { get; set; } = new();
    }
}
