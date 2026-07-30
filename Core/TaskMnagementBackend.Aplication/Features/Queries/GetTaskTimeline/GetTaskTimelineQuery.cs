using MediatR;
using System.Collections.Generic;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetTaskTimeline
{
    public class GetTaskTimelineQuery : IRequest<List<TaskTimelineDto>>
    {
        public int TaskId { get; set; }
    }
}