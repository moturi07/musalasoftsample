using Microsoft.AspNetCore.Identity;

namespace MusalaDAL.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string Surname { get; set; }
    }
}
