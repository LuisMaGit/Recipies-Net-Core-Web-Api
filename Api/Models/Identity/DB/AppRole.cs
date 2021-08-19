using Microsoft.AspNetCore.Identity;

namespace Api.Models.Identity.DB
{
    //Cambio del key en identity rol a entero
    public class AppRole : IdentityRole<int>
    {
    }
}