using System.Globalization;

namespace School.Data.Commons
{
    public static class LocalizableEntityExtensions
    {
        public static string GetLocalizedName(this ILocalizableEntity entity)
        {
            if (entity is null) return string.Empty;

            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            // Get First 2 Letters In Word [ar-EG --> ar]
            return culture.TwoLetterISOLanguageName.ToLower() == "ar"
                ? entity.NameAr
                : entity.NameEn;
        }
    }
}
