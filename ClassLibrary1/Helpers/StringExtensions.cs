using System.Text.RegularExpressions;

namespace ClassLibrary1.Helpers
{
    public static class StringExtensions
    {
        public static string Sanitize(
            this string value,
            string pattern)
        {
            var regex = new Regex(pattern);

            var result = regex.Replace(value, "");

            return result;
        }
    }
}
