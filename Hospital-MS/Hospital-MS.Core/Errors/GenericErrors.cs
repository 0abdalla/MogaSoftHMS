using Hospital_MS.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Errors
{
    public class GenericErrors<T>
    {
        private static string EntityName => typeof(T).Name;

        public static Error NotFound
            = new($"{EntityName}.NotFound", $"The {EntityName} with this ID was not found", StatusCodes.Status404NotFound);

        public static Error NotAvailable
            = new($"{EntityName}.NotAvailable", $"The {EntityName} is not available now", StatusCodes.Status400BadRequest);

        public static Error DuplicateEntry
            = new($"{EntityName}.DuplicateEntry", $"A {EntityName} with these details already exists", StatusCodes.Status409Conflict);

        public static Error FailedToAdd
            = new($"{EntityName}.FailedToAdd", $"Failed to add the {EntityName}", StatusCodes.Status400BadRequest);

        public static Error FailedToUpdate
            = new($"{EntityName}.FailedToUpdate", $"Failed to update the {EntityName}", StatusCodes.Status400BadRequest);

        public static Error FailedToDelete
            = new($"{EntityName}.FailedToDelete", $"Failed to delete the {EntityName}", StatusCodes.Status400BadRequest);

        public static Error InvalidCredentials
            = new($"{EntityName}.InvalidCredentials", $"اسم المستخدم او كلمة المرور غير صالح", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicateEmail
            = new($"{EntityName}.DuplicateEmail", "البريد الإلكتروني مسجل مسبقاً", StatusCodes.Status409Conflict);


    }
}
