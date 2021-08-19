using System.ComponentModel.DataAnnotations;

namespace Api.ModelsDto.Requests.Identity
{
    public class LoginDto
    {
        [Required] 
        [EmailAddress] 
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}