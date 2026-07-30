using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TeamInvitation;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Queries.GetPendingInvitations
{
   public class GetPendingInvitationsResponse : OperationResultBase
    {
        public IEnumerable<TeamInvitationDto> Invitations { get; set; }
            = new List<TeamInvitationDto>();
    }
}
