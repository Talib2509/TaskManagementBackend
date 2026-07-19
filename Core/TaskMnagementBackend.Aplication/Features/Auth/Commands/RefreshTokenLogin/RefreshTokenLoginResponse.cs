namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.RefreshTokenLogin
{
    public class RefreshTokenLoginResponse
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }

        public string? AccessToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
