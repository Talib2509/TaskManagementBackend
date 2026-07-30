
﻿using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

﻿using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;


namespace TaskMnagementBackend.Domain.Entities
{
    public class Notification : BaseEntity
    {

        
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } 

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

        public string Title { get; set; } = string.Empty;
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public Guid UserId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;

        public AppUser? User { get; set; }

        public Guid? RelatedEntityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

