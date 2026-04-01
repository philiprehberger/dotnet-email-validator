namespace Philiprehberger.EmailValidator;

/// <summary>
/// Result of email address validation.
/// </summary>
/// <param name="IsValid">Whether the email address is valid.</param>
/// <param name="Error">Error message if invalid, null if valid.</param>
/// <param name="NormalizedAddress">Lowercased, trimmed email if valid, null if invalid.</param>
/// <param name="Suggestion">Suggested corrected email if a domain typo was detected, null otherwise.</param>
/// <param name="IsDisposable">Whether the email domain belongs to a known disposable email provider.</param>
public record ValidationResult(bool IsValid, string? Error, string? NormalizedAddress, string? Suggestion = null, bool IsDisposable = false);
