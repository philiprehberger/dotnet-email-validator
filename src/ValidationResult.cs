namespace Philiprehberger.EmailValidator;

/// <summary>
/// Result of email address validation.
/// </summary>
/// <param name="IsValid">Whether the email address is valid.</param>
/// <param name="Error">Error message if invalid, null if valid.</param>
/// <param name="NormalizedAddress">Lowercased, trimmed email if valid, null if invalid.</param>
public record ValidationResult(bool IsValid, string? Error, string? NormalizedAddress);
