using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Extensions
{
    public static class DataRowExtensions
    {
        public static void TryTranslateEnum<TEnum>(this DataRow row, string columnName) where TEnum : struct, Enum
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                var enumStr = row[columnName].ToString();
                if (Enum.TryParse<TEnum>(enumStr, out var enumValue))
                {
                    var arabicValue = enumValue.GetArabicValue();
                    row[columnName] = arabicValue;
                }
            }
        }

        public static string? TryTranslateEnum<TEnum>(this string? value) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (Enum.TryParse<TEnum>(value, out var enumValue))
            {
                return enumValue.GetArabicValue();
            }

            return value;
        }
    }
}
