using System.Globalization;

namespace School.Data.Commons
{
    public static class LocalizationHelper
    {
        // More General Method
        public static string GetLocalized(string textAr, string textEn)
        {
            if (string.IsNullOrWhiteSpace(textAr) && string.IsNullOrWhiteSpace(textEn))
                return string.Empty;

            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            return culture.TwoLetterISOLanguageName.ToLower() == "ar"
                ? (string.IsNullOrWhiteSpace(textAr) ? textEn : textAr)
                : (string.IsNullOrWhiteSpace(textEn) ? textAr : textEn);
        }
    }

}
