using MediatR;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetTeamMemberById
{
    public class GetTeamMemberByIdRequest : IRequest<GetTeamMemberByIdResponse>
    {
        public int Id { get; set; }
    }
}