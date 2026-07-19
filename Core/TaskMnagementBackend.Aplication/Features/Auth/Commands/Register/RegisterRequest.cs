using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.Register
{
    public class RegisterRequest : IRequest<RegisterResponse>
    {
        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string CompanyName { get; set; } = default!; 
    }
}
