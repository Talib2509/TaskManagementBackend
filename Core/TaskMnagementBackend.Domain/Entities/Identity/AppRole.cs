using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<Endpoint> Endpoint { get; set; } = new List<Endpoint>();
    }
}
