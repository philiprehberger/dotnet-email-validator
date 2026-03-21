using System.Globalization;
using System.Net;
using System.Text;

namespace Philiprehberger.EmailValidator;

/// <summary>
/// Provides RFC 5321/5322 compliant email address validation with international domain support.
/// </summary>
public static class EmailValidator
{
    private const int MaxEmailLength = 254;
    private const int MaxLocalPartLength = 64;
    private const int MaxDomainLength = 255;
    private const int MaxDomainLabelLength = 63;

    /// <summary>
    /// Characters permitted in the unquoted local part of an email address per RFC 5321/5322.
    /// </summary>
    private static readonly HashSet<char> AllowedLocalSpecialChars =
        ['!', '#', '$', '%', '&', '\'', '*', '+', '-', '/', '=', '?', '^', '_', '`', '{', '|', '}', '~'];

    /// <summary>
    /// Checks whether the specified email address is valid.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns><c>true</c> if the email address is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValid(string email)
    {
        return Validate(email).IsValid;
    }

    /// <summary>
    /// Validates the specified email address and returns a detailed result.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> containing the validation outcome, any error message, and the normalized address.</returns>
    public static ValidationResult Validate(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Fail("Email address must not be empty.");
        }

        var trimmed = email.Trim();

        if (trimmed.Length > MaxEmailLength)
        {
            return Fail($"Email address exceeds maximum length of {MaxEmailLength} characters.");
        }

        var atIndex = trimmed.LastIndexOf('@');

        if (atIndex < 0)
        {
            return Fail("Email address must contain an '@' character.");
        }

        if (atIndex == 0)
        {
            return Fail("Local part must not be empty.");
        }

        if (atIndex == trimmed.Length - 1)
        {
            return Fail("Domain part must not be empty.");
        }

        var localPart = trimmed[..atIndex];
        var domainPart = trimmed[(atIndex + 1)..];

        var localError = ValidateLocalPart(localPart);
        if (localError is not null)
        {
            return Fail(localError);
        }

        var domainError = ValidateDomainPart(domainPart);
        if (domainError is not null)
        {
            return Fail(domainError);
        }

        var normalizedDomain = NormalizeDomain(domainPart);
        var normalized = $"{localPart}@{normalizedDomain}".ToLowerInvariant();

        return new ValidationResult(true, null, normalized);
    }

    /// <summary>
    /// Validates the local part of an email address.
    /// </summary>
    /// <param name="localPart">The local part (before the @).</param>
    /// <returns>An error message if invalid; <c>null</c> if valid.</returns>
    private static string? ValidateLocalPart(string localPart)
    {
        if (localPart.Length > MaxLocalPartLength)
        {
            return $"Local part exceeds maximum length of {MaxLocalPartLength} characters.";
        }

        if (localPart.StartsWith('"') && localPart.EndsWith('"'))
        {
            return ValidateQuotedLocalPart(localPart);
        }

        return ValidateUnquotedLocalPart(localPart);
    }

    /// <summary>
    /// Validates a quoted local part (surrounded by double quotes).
    /// </summary>
    /// <param name="localPart">The full quoted local part including surrounding quotes.</param>
    /// <returns>An error message if invalid; <c>null</c> if valid.</returns>
    private static string? ValidateQuotedLocalPart(string localPart)
    {
        var inner = localPart[1..^1];

        for (var i = 0; i < inner.Length; i++)
        {
            var c = inner[i];

            if (c == '\\')
            {
                if (i + 1 >= inner.Length)
                {
                    return "Quoted local part contains a trailing backslash with no escaped character.";
                }

                var next = inner[i + 1];
                if (next < 32 || next > 126)
                {
                    return "Quoted local part contains an invalid escaped character.";
                }

                i++;
                continue;
            }

            if (c == '"')
            {
                return "Quoted local part contains an unescaped double quote.";
            }

            if (c < 32 || c > 126)
            {
                return "Quoted local part contains an invalid character.";
            }
        }

        return null;
    }

    /// <summary>
    /// Validates an unquoted local part.
    /// </summary>
    /// <param name="localPart">The unquoted local part.</param>
    /// <returns>An error message if invalid; <c>null</c> if valid.</returns>
    private static string? ValidateUnquotedLocalPart(string localPart)
    {
        if (localPart.StartsWith('.') || localPart.EndsWith('.'))
        {
            return "Local part must not start or end with a dot.";
        }

        if (localPart.Contains(".."))
        {
            return "Local part must not contain consecutive dots.";
        }

        foreach (var c in localPart)
        {
            if (char.IsLetterOrDigit(c) || c == '.' || AllowedLocalSpecialChars.Contains(c))
            {
                continue;
            }

            return $"Local part contains an invalid character: '{c}'.";
        }

        return null;
    }

    /// <summary>
    /// Validates the domain part of an email address, including IP address literals and internationalized domain names.
    /// </summary>
    /// <param name="domainPart">The domain part (after the @).</param>
    /// <returns>An error message if invalid; <c>null</c> if valid.</returns>
    private static string? ValidateDomainPart(string domainPart)
    {
        if (domainPart.StartsWith('[') && domainPart.EndsWith(']'))
        {
            return ValidateIpAddressLiteral(domainPart);
        }

        return ValidateHostnameDomain(domainPart);
    }

    /// <summary>
    /// Validates an IP address literal domain (e.g., [192.168.1.1] or [IPv6:::1]).
    /// </summary>
    /// <param name="domainPart">The domain part including square brackets.</param>
    /// <returns>An error message if invalid; <c>null</c> if valid.</returns>
    private static string? ValidateIpAddressLiteral(string domainPart)
    {
        var inner = domainPart[1..^1];

        if (inner.StartsWith("IPv6:", StringComparison.OrdinalIgnoreCase))
        {
            var ipv6 = inner[5..];
            if (!IPAddress.TryParse(ipv6, out var addr) || addr.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                return "Domain contains an invalid IPv6 address literal.";
            }

            return null;
        }

        if (!IPAddress.TryParse(inner, out var ipAddr) || ipAddr.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return "Domain contains an invalid IP address literal.";
        }

        return null;
    }

    /// <summary>
    /// Validates a hostname-style domain, including internationalized domain names (IDN).
    /// </summary>
    /// <param name="domainPart">The domain part as a hostname.</param>
    /// <returns>An error message if invalid; <c>null</c> if valid.</returns>
    private static string? ValidateHostnameDomain(string domainPart)
    {
        string asciiDomain;

        try
        {
            var idn = new IdnMapping { AllowUnassigned = true, UseStd3AsciiRules = true };
            asciiDomain = idn.GetAscii(domainPart);
        }
        catch (ArgumentException)
        {
            return "Domain contains invalid characters or is not a valid internationalized domain name.";
        }

        if (asciiDomain.Length > MaxDomainLength)
        {
            return $"Domain exceeds maximum length of {MaxDomainLength} characters.";
        }

        var labels = asciiDomain.Split('.');

        if (labels.Length < 2)
        {
            return "Domain must contain at least two labels (e.g., 'example.com').";
        }

        foreach (var label in labels)
        {
            if (string.IsNullOrEmpty(label))
            {
                return "Domain must not contain empty labels.";
            }

            if (label.Length > MaxDomainLabelLength)
            {
                return $"Domain label '{label}' exceeds maximum length of {MaxDomainLabelLength} characters.";
            }

            if (label.StartsWith('-') || label.EndsWith('-'))
            {
                return "Domain labels must not start or end with a hyphen.";
            }

            foreach (var c in label)
            {
                if (!char.IsLetterOrDigit(c) && c != '-')
                {
                    return $"Domain label contains an invalid character: '{c}'.";
                }
            }
        }

        var tld = labels[^1];
        if (tld.All(char.IsDigit))
        {
            return "Top-level domain must not be entirely numeric.";
        }

        return null;
    }

    /// <summary>
    /// Normalizes the domain part by converting internationalized domain names to their ASCII representation and lowering case.
    /// </summary>
    /// <param name="domainPart">The domain part to normalize.</param>
    /// <returns>The normalized domain in lowercase ASCII form.</returns>
    private static string NormalizeDomain(string domainPart)
    {
        if (domainPart.StartsWith('['))
        {
            return domainPart;
        }

        try
        {
            var idn = new IdnMapping { AllowUnassigned = true, UseStd3AsciiRules = true };
            return idn.GetAscii(domainPart).ToLowerInvariant();
        }
        catch (ArgumentException)
        {
            return domainPart.ToLowerInvariant();
        }
    }

    /// <summary>
    /// Creates a failed validation result with the specified error message.
    /// </summary>
    /// <param name="error">The error message describing why validation failed.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating failure.</returns>
    private static ValidationResult Fail(string error)
    {
        return new ValidationResult(false, error, null);
    }
}
