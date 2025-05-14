using Microsoft.AspNetCore.Identity;

namespace Hospital_MS.Core.Models;
public class ApplicationRole : IdentityRole
{
    public bool IsDeleted { get; set; }
}
