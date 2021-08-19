namespace Api.ModelsDto.Responses.Identity
{
    public class SuccesAuthResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
        public bool Success { get; set; }
    }
}