using Microsoft.AspNetCore.Identity;

using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Infrastructure.Extension
{
    public class BCryptPasswordHasher : IPasswordHasher<AppUser>
    {
        public string HashPassword(AppUser user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public PasswordVerificationResult VerifyHashedPassword(
            AppUser user,
            string hashedPassword,
            string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
                return PasswordVerificationResult.Failed;

            try
            {
                var isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);

                return isValid
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed;
            }
            catch
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}
