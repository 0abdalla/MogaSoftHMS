


using Microsoft.AspNetCore.Identity;

namespace Hospital_MS.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int? StaffId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LoginDate { get; set; }
    }
}
