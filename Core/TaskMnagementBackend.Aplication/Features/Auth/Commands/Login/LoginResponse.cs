namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.Login
{
    public class LoginResponse
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? RefreshToken { get; set; }
    }
}
