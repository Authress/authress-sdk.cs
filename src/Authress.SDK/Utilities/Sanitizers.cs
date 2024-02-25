using System;
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

        internal static string SanitizeIssuerUrl(string rawUrlString) {
            var sanitizedUrlString = rawUrlString;
            if (!sanitizedUrlString.StartsWith("http")) {
                sanitizedUrlString = Regex.IsMatch(sanitizedUrlString, @"^(localhost|authress.localhost.localstack.cloud:4566$)") ? $"http://{sanitizedUrlString}" : $"https://{sanitizedUrlString}";
            }

            var sanitizedUrl = new Uri(sanitizedUrlString);
            var domainBaseUrlMatch = Regex.Match(sanitizedUrl.GetLeftPart(UriPartial.Authority), @"^https?://([a-z0-9-]+)[.][a-z0-9-]+[.]authress[.]io$");
            if (domainBaseUrlMatch.Success) {
                var newSanitizedUrl = new UriBuilder(sanitizedUrl)
                {
                    Host = $"{domainBaseUrlMatch.Groups[1].Value}.login.authress.io"
                };
                sanitizedUrlString = newSanitizedUrl.Uri.ToString();
            }

            return sanitizedUrlString.Replace(@"[/]+$", "");
        }
    }
}
