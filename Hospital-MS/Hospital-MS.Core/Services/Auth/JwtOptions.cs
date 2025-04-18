using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Services.Auth
{
    public class JwtOptions
    {
        public static string SectionName { get; set; } = "Jwt";

        [Required]
        public string Key { get; init; } = string.Empty;

        [Required]
        public string Issuer { get; init; } = string.Empty;

        [Required]
        public string Audience { get; init; } = string.Empty;

        [Required, Range(1, int.MaxValue, ErrorMessage = "Invalid Expiry Minutes")]

        public int ExpiryMinutes { get; init; }
    }
}
