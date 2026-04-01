namespace Philiprehberger.EmailValidator;

/// <summary>
/// Suggests corrections for common email domain typos.
/// </summary>
internal static class TypoSuggestion
{
    private static readonly string[] CommonDomains =
    [
        "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "aol.com",
        "icloud.com", "mail.com", "protonmail.com", "zoho.com", "yandex.com",
        "gmx.com", "live.com", "msn.com", "me.com", "mac.com",
        "comcast.net", "verizon.net", "att.net", "sbcglobal.net", "cox.net"
    ];

    /// <summary>
    /// Suggests a corrected domain if the input domain is a likely typo of a common provider.
    /// </summary>
    /// <param name="domain">The domain to check for typos.</param>
    /// <returns>The suggested correct domain, or <c>null</c> if no suggestion is available.</returns>
    internal static string? SuggestDomain(string domain)
    {
        var lower = domain.ToLowerInvariant();

        if (CommonDomains.Contains(lower))
        {
            return null;
        }

        string? bestMatch = null;
        var bestDistance = int.MaxValue;

        foreach (var known in CommonDomains)
        {
            var distance = LevenshteinDistance(lower, known);

            if (distance > 0 && distance <= 2 && distance < bestDistance)
            {
                bestDistance = distance;
                bestMatch = known;
            }
        }

        return bestMatch;
    }

    /// <summary>
    /// Computes the Levenshtein edit distance between two strings using the Wagner-Fischer algorithm.
    /// </summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <returns>The minimum number of single-character edits required to transform one string into the other.</returns>
    private static int LevenshteinDistance(string a, string b)
    {
        var m = a.Length;
        var n = b.Length;
        var dp = new int[m + 1, n + 1];

        for (var i = 0; i <= m; i++)
        {
            dp[i, 0] = i;
        }

        for (var j = 0; j <= n; j++)
        {
            dp[0, j] = j;
        }

        for (var i = 1; i <= m; i++)
        {
            for (var j = 1; j <= n; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;

                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost);
            }
        }

        return dp[m, n];
    }
}
