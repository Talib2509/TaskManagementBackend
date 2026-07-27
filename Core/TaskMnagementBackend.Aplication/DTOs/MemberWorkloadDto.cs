using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class MemberWorkloadDto
    {
        public Guid UserId { get; set; }
        public int TaskCount { get; set; }
    }
}
