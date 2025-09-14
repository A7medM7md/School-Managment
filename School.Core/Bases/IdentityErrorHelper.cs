using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace School.Core.Bases
{
    public static class IdentityErrorHelper
    {
        public static string LocalizeErrors(IEnumerable<IdentityError> errors,
                                                IStringLocalizer localizer
        )
        {
            return string.Join(" | ", errors.Select(e =>
            {
                var localized = localizer[e.Code];
                return localized.ResourceNotFound ? e.Description : localized.Value;
            }));
        }
    }
}
