using System;

namespace TaskMnagementBackend.Aplication.DTOs.Team
{
    public class CreateTeamDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CompanyId { get; set; }

        public Guid? TeamLeadId { get; set; }
    }
}
