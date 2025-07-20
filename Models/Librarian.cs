using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Models
{
    public class Librarian : IdentityUser<int>
    {
        // You can add additional custom properties here if needed.
        // For example:
        // public string FullName { get; set; }
        
        // But UserName, PasswordHash, Email, etc. are inherited from IdentityUser
    }
}

