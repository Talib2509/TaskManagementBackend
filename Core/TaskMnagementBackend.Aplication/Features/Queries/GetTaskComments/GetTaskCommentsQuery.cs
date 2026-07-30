using MediatR;
using System;
using System.Collections.Generic;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetTaskComments
{
    public class GetTaskCommentsQuery : IRequest<List<TaskCommentDto>>
    {
        public int TaskId { get; set; }

        public GetTaskCommentsQuery(int taskId)
        {
            TaskId = taskId;
        }
    }
}