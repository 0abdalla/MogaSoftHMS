using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.Common
{
    public static class ArabicNormalizer
    {
        public static string NormalizeArabic(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input
                .Replace("أ", "ا")
                .Replace("إ", "ا")
                .Replace("آ", "ا")
                .Replace("ى", "ي")
                .Replace("ئ", "ي")
                .Replace("ؤ", "و")
                .Replace("ة", "ه")
                .ToLower();
        }
    }
}
