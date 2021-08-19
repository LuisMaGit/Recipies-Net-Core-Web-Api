using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.ModelsDto.Requests.Identity
{
    public class RegisterDto
    {
        [StringLength(50)] 
        public string FirstName { get; set; }
        [StringLength(50)] 
        public string LastName { get; set; }
        [Required] 
        [EmailAddress] 
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}