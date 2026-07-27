using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetTeamInvitationById;

namespace TaskMnagementBackend.Aplication.Features.Invitations.Queries.GetPendingInvitations
{
   public class GetPendingInvitationsRequest: IRequest<GetPendingInvitationsResponse>
    {
        public Guid UserId { get; set; }
    }
}
