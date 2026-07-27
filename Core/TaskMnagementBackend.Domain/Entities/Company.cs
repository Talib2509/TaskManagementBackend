using System;
using System.Collections.Generic;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{

    public class Company : BaseEntity
    {
        public Company()
        {
            Teams = new List<Team>();
        }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }


        public Guid OwnerId { get; set; }

        public AppUser? Owner { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}
