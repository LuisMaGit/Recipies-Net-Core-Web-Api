using System;

namespace Api.Configuration
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}