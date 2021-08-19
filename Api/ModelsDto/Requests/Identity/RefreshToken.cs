using System;

namespace Api.ModelsDto.Requests.Identity
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}