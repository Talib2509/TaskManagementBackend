using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{
    public class Endpoint : BaseEntity
    {
        public Endpoint()
        {
            Roles = new HashSet<AppRole>();
        }

        public string HttpMethod { get; set; } = string.Empty;

        public string RouteTemplate { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;


        public string Definition { get; set; } = string.Empty;

        public string Menu { get; set; } = string.Empty;

        public ICollection<AppRole> Roles { get; set; }
    }
}
