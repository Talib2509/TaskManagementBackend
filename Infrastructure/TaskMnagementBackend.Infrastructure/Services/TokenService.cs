
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = GetConfigValue("JwtSettings:Secret");
            _issuer = GetConfigValue("JwtSettings:Issuer");
            _audience = GetConfigValue("JwtSettings:Audience");

            if (_secretKey.Length < 32)
            {
                throw new InvalidOperationException(
                    "JwtSettings:Secret must be at least 32 characters long for HS256.");
            }
        }

        public Task<string> CreateAccessTokenAsync(
            AppUser user,
            IList<string> roles,
            DateTime expireDate)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim("Email", user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.FullName ?? string.Empty)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secretKey));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expireDate,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        public DateTime CreateAccessTokenExpireDate()
        {
            var minutes = GetIntConfigValue("JwtSettings:AccessTokenMinutes", 15);
            return DateTime.UtcNow.AddMinutes(minutes);
        }

        public DateTime CreateRefreshTokenExpireDate()
        {
            var days = GetIntConfigValue("JwtSettings:RefreshTokenDays", 7);
            return DateTime.UtcNow.AddDays(days);
        }

        private string GetConfigValue(string key)
        {
            var value = _configuration[key];

            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"{key} tapılmadı.");

            var envValue = _configuration[value];

            return string.IsNullOrWhiteSpace(envValue)
                ? value
                : envValue;
        }

        private int GetIntConfigValue(string key, int defaultValue)
        {
            var value = _configuration[key];

            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            var envValue = _configuration[value];

            var finalValue = string.IsNullOrWhiteSpace(envValue)
                ? value
                : envValue;

            return int.TryParse(finalValue, out var result)
                ? result
                : defaultValue;
        }
    }
}
