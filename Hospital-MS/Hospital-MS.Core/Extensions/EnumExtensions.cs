using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetArabicValue(this Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString()).FirstOrDefault();
            var attr = member?.GetCustomAttribute<EnumMemberAttribute>();
            return attr?.Value ?? value.ToString();
        }
    }
}
