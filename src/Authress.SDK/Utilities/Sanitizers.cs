using System.Text.RegularExpressions;

namespace Authress.SDK.Utilities {
    internal static class Sanitizers
    {
        internal static string SanitizeUrl(string urlString) {
            if (string.IsNullOrEmpty(urlString)) {
                return null;
            }

            if (urlString.StartsWith("http")) {
                return urlString;
            }

            if (Regex.IsMatch(urlString, @"^localhost", RegexOptions.IgnoreCase)) {
                return $"http://{urlString}";
            }

            return $"https://{urlString}";
        }
    }
}
