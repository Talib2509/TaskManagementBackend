using System;

namespace TaskMnagementBackend.Aplication.DTOs.Team
{
    public class UpdateTeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public Guid? TeamLeadId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
