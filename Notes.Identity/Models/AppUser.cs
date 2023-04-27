using Microsoft.AspNetCore.Identity;

namespace Notes.Identity.Models
{
    public class AppUser: IdentityUser
    {
        public string UserName{ get; set; }   
        public string LastName { get; set; }
    }
}
