using System;
using System.Threading.Tasks;
using Api.Models.Identity;

namespace Api.Services.IdentityService
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(Register register);

        Task<AuthenticationResult> LoginAsync(Login login);

        Task<AuthenticationResult> RefreshTokenAsync(string token, Guid refreshToken);
    }
}