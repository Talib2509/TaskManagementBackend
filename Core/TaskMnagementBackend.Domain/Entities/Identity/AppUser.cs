using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public string CompanyName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ActiveTeamId { get; set; }
    }
}
