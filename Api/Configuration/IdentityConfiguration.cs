namespace Api.Configuration
{
    public class IdentityConfiguration
    {
        public bool UniqueEmail { get; set; }
        public bool RequiredDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public bool RequireUppercase { get; set; }
        public int RequiredLength { get; set; }
    }
}