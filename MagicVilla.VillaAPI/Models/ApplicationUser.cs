using Microsoft.AspNetCore.Identity;

namespace MagicVilla.VillaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; }
    }
}
