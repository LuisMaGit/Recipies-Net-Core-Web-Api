using System.Collections.Generic;

namespace Api.ModelsDto.Responses.Identity
{
    public class ErrorAuthResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}