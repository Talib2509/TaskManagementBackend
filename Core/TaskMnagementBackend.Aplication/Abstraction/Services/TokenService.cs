using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(
            AppUser user,
            IList<string> roles,
            DateTime expireDate);

        string GenerateRefreshToken();

        DateTime CreateAccessTokenExpireDate();

        DateTime CreateRefreshTokenExpireDate();
    }
}
