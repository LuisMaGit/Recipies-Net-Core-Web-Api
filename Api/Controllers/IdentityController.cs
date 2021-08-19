using System.Threading.Tasks;
using Api.Filters.Common;
using Api.Models.Identity;
using Api.ModelsDto.Requests.Identity;
using Api.ModelsDto.Responses.Identity;
using Api.Services.IdentityService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [ValidateModelFilter]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapperService;

        public IdentityController(IIdentityService identityService, IMapper mapperService)
        {
            _identityService = identityService;
            _mapperService = mapperService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var register = _mapperService.Map<Register>(dto);
            var authResult = await _identityService.RegisterAsync(register);
            return _handleAuthResult(authResult);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var login = _mapperService.Map<Login>(dto);
            var authResult = await _identityService.LoginAsync(login);
            return _handleAuthResult(authResult);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var authResult = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return _handleAuthResult(authResult);
        }

        private IActionResult _handleAuthResult(AuthenticationResult authResult)
        {
            if (!authResult.Success)
            {
                return BadRequest(new ErrorAuthResponse
                {
                    Errors = authResult.Errors,
                });
            }

            return Ok(new SuccesAuthResponse
            {
                Success = true,
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }
    }
}