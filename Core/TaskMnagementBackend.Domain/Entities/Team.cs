using System;
using System.Collections.Generic;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{

    public class Team : BaseEntity
    {
        public Team()
        {
            TeamMembers = new List<TeamMember>();
            TaskItems = new List<TaskItem>();
        }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CompanyId { get; set; }

        public Company? Company { get; set; }

       
        public Guid? TeamLeadId { get; set; }

        public AppUser? TeamLead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        public ICollection<TeamMember> TeamMembers { get; set; }

        public ICollection<TaskItem> TaskItems { get; set; }
    }
}
