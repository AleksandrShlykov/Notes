using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Notes.Identity.Models
{
    public class AppUser : IdentityUser
    {
        
        public string? FirstName{ get; set; }
      
        public string? LastName { get; set; }
    }
}
