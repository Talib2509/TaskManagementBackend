namespace TaskMnagementBackend.Aplication.DTOs.TeamMember
{
    public class RemoveTeamMemberResultDto
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool TasksTransferred { get; set; }

        public Guid? NewAssigneeId { get; set; }
    }
}