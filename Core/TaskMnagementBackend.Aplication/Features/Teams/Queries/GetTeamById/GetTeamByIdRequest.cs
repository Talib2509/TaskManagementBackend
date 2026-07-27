using MediatR;



namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetTeamById
{
    public class GetTeamByIdRequest : IRequest<GetTeamByIdResponse>
    {
        public int Id { get; set; }
    }
}