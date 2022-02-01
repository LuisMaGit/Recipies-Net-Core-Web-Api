using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Models;
using Api.Models.Identity;
using Api.Models.Identity.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOptions<JwtConfiguration> _jwtConfigutarion;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly RecipiesDbContext _dataContext;

        public IdentityService(UserManager<AppUser> userManager, IOptions<JwtConfiguration> jwtConfigutarion,
            TokenValidationParameters tokenValidationParameters, RecipiesDbContext dataContext)
        {
            _userManager = userManager;
            _jwtConfigutarion = jwtConfigutarion;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
        }

        private AppUser _user;

        //Construct Token/Refresh token from _user
        private async Task<AuthenticationResult> _HandleTokenResult()
        {
            //Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key =
                Encoding.UTF8.GetBytes(_jwtConfigutarion.Value.Secret);
            var issuer = _jwtConfigutarion.Value.Issuer;
            var lifeTime = _jwtConfigutarion.Value.LifeTime;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("id", _user.Id.ToString()),
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(lifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256),
            };

            var rawToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(rawToken);
            //Refresh Token
            var rawRefreshToken = new RefreshToken
            {
                JwtId = rawToken.Id,
                UserId = _user.Id,
                CreateionDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddMonths(6)
            };

            await _dataContext.RefreshToken.AddAsync(rawRefreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                RefreshToken = rawRefreshToken.Token.ToString(),
            };
        }

        private async Task<AuthenticationResult> _CheckRegisterExitsEmail(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] {"Email already exists"}
                };
            }

            return null;
        }

        private async Task<AuthenticationResult> _HandleRegistration(Register register)
        {
            _user = new AppUser()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email,
                Email = register.Email,
            };

            var identityResult = await _userManager.CreateAsync(_user, register.Password);

            if (identityResult.Succeeded == false)
            {
                return new AuthenticationResult()
                {
                    Errors = identityResult.Errors.Select(e => e.Description),
                };
            }

            return null;
        }

        private async Task<AuthenticationResult> _CheckLoginEmailExists(string email)
        {
            _user = await _userManager.FindByEmailAsync(email);

            if (_user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User does not exits"}
                };
            }

            return null;
        }

        private async Task<AuthenticationResult> _CheckPasswordIsCorrect(string password)
        {
            var passwordOk = await _userManager.CheckPasswordAsync(_user, password);
            if (!passwordOk)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"Incorrect password"}
                };
            }

            return null;
        }

        //Get the principal claims from the token
        private ClaimsPrincipal _GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                return !_IsJwtValidSecurityAlgorithm(validatedToken) ? null : principal;
            }
            catch
            {
                return null;
            }
        }

        //Validates the SecurityKey and Alghorithm of the token
        private static bool _IsJwtValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurity) &&
                   jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResult> RegisterAsync(Register register)
        {
            var userAlreadyExistsResult = await _CheckRegisterExitsEmail(register.Email);
            if (userAlreadyExistsResult != null)
            {
                return userAlreadyExistsResult;
            }

            var errorRegistrationResult = await _HandleRegistration(register);
            return errorRegistrationResult ?? await _HandleTokenResult();
        }

        public async Task<AuthenticationResult> LoginAsync(Login login)
        {
            var userDoesNotExistsResponse = await _CheckLoginEmailExists(login.Email);

            if (userDoesNotExistsResponse != null)
            {
                return userDoesNotExistsResponse;
            }

            var wrongPasswordResponse = await _CheckPasswordIsCorrect(login.Password);

            return wrongPasswordResponse ?? await _HandleTokenResult();
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, Guid refreshToken)
        {
            //VALIDATE OLD TOKEN, AND GET PRINCIPALS FROM IT
            var claimsPrincipal = _GetPrincipalFromToken(token);
            if (claimsPrincipal == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"Invalid token"}
                };
            }

            //CHECK IF THE OLD TOKEN HAS EXPIRE
            var expirationDateUnix =
                long.Parse(claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expirationDateTimeUtc =
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expirationDateUnix);

            if (expirationDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"This token hasn't expired yet"}
                };
            }

            //GET REFRESH TOKEN
            var storedRefreshToken = await _dataContext.RefreshToken.SingleAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"This token don't exits"}
                };
            }

            //CHECK EXPIRATIONDATE, INVALIDATED, USED IN REFRESH TOKEN
            if (DateTime.UtcNow > storedRefreshToken.ExpireDate)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"This refresh token has expire"}
                };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"This has been invalidated"}
                };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"This token has been used"}
                };
            }

            //CHECK IF THE OLD TOKEN HAS THE SAME JWT_ID AS THE REFRESH TOKEN
            var jwtId = claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedRefreshToken.JwtId != jwtId)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"This refresh token does not match JWT"}
                };
            }

            //SET AS USED THE NEW REFRESH TOKEN IN THE DB
            storedRefreshToken.Used = true;
            _dataContext.RefreshToken.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

            //FIND THE USER THAT OF THE TOKEN AND CREATE A NEW TOKEN, REFRESH TOKEN RESPONSE
            _user = await _userManager.FindByIdAsync(claimsPrincipal.Claims.Single(c => c.Type == "id").Value);

            return await _HandleTokenResult();
        }
    }
}