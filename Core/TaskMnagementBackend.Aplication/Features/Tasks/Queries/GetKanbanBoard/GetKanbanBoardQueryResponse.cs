using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Queries.GetKanbanBoard
{
    public class GetKanbanBoardQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public KanbanBoardDto Board { get; set; } = new();
    }
}
