namespace Philiprehberger.EmailValidator;

/// <summary>
/// Provides a list of known disposable and temporary email providers.
/// </summary>
internal static class DisposableDomains
{
    private static readonly HashSet<string> Domains = new(StringComparer.OrdinalIgnoreCase)
    {
        "mailinator.com", "guerrillamail.com", "tempmail.com", "throwaway.email",
        "yopmail.com", "sharklasers.com", "guerrillamailblock.com", "grr.la",
        "guerrillamail.info", "guerrillamail.net", "guerrillamail.org", "guerrillamail.de",
        "tempail.com", "dispostable.com", "trashmail.com", "trashmail.me",
        "trashmail.net", "mailnesia.com", "maildrop.cc", "discard.email",
        "mailcatch.com", "temp-mail.org", "fakeinbox.com", "tempinbox.com",
        "getnada.com", "emailondeck.com", "33mail.com",
        "mytemp.email", "mohmal.com", "burnermail.io", "inboxkitten.com",
        "minutemail.com", "temp-mail.io", "tempr.email", "10minutemail.com",
        "mailsac.com", "harakirimail.com", "crazymailing.com", "tmail.ws",
        "tempmailo.com", "spamgourmet.com", "mailexpire.com", "safetymail.info",
        "filzmail.com", "jetable.org", "mailnull.com", "mailscrap.com",
        "throwam.com", "trashymail.com", "binkmail.com", "bobmail.info"
    };

    /// <summary>
    /// Determines whether the specified domain belongs to a known disposable email provider.
    /// </summary>
    /// <param name="domain">The domain to check.</param>
    /// <returns><c>true</c> if the domain is a known disposable provider; otherwise, <c>false</c>.</returns>
    internal static bool IsDisposable(string domain)
    {
        return Domains.Contains(domain);
    }
}
