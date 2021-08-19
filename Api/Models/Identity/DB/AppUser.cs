using Microsoft.AspNetCore.Identity;

namespace Api.Models.Identity.DB
{
    //Cambio del IdentityUser con dos campos y el key a entero
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}