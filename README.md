# Philiprehberger.EmailValidator

[![CI](https://github.com/philiprehberger/dotnet-email-validator/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-email-validator/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.EmailValidator.svg)](https://www.nuget.org/packages/Philiprehberger.EmailValidator)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-email-validator)](https://github.com/philiprehberger/dotnet-email-validator/commits/main)

RFC 5321/5322 compliant email validation with typo suggestions, disposable email detection, and bulk validation.

## Installation

```bash
dotnet add package Philiprehberger.EmailValidator
```

## Usage

### Quick Check

```csharp
using Philiprehberger.EmailValidator;

bool valid = EmailValidator.IsValid("user@example.com");
// true

bool invalid = EmailValidator.IsValid("not-an-email");
// false
```

### Detailed Validation

```csharp
using Philiprehberger.EmailValidator;

var result = EmailValidator.Validate("user@example.com");
// result.IsValid            → true
// result.Error               → null
// result.NormalizedAddress   → "user@example.com"

var failed = EmailValidator.Validate("user@.com");
// failed.IsValid  → false
// failed.Error    → "Domain must not contain empty labels."
```

### Internationalized Domains

```csharp
using Philiprehberger.EmailValidator;

var result = EmailValidator.Validate("user@münchen.de");
// result.IsValid            → true
// result.NormalizedAddress   → "user@xn--mnchen-3ya.de"
```

### Typo Suggestions

```csharp
using Philiprehberger.EmailValidator;

var result = EmailValidator.ValidateWithSuggestion("user@gmial.com");
// result.IsValid    → true
// result.Suggestion → "user@gmail.com"
```

### Disposable Email Detection

```csharp
using Philiprehberger.EmailValidator;

bool disposable = EmailValidator.IsDisposable("user@mailinator.com");
// true

bool legit = EmailValidator.IsDisposable("user@gmail.com");
// false
```

### Bulk Validation

```csharp
using Philiprehberger.EmailValidator;

var results = EmailValidator.ValidateMany(new[] { "a@b.com", "invalid", "c@d.org" });
// results[0].IsValid → true
// results[1].IsValid → false
// results[2].IsValid → true
```

## API

| Member | Returns | Description |
|--------|---------|-------------|
| `EmailValidator.IsValid(string email)` | `bool` | Returns whether the email address is valid. |
| `EmailValidator.Validate(string email)` | `ValidationResult` | Returns a detailed validation result with error and normalized address. |
| `ValidationResult.IsValid` | `bool` | Whether the email address is valid. |
| `ValidationResult.Error` | `string?` | Error message if invalid, null if valid. |
| `ValidationResult.NormalizedAddress` | `string?` | Lowercased, trimmed email if valid, null if invalid. |
| `ValidationResult.Suggestion` | `string?` | Suggested corrected email if a domain typo was detected, null otherwise. |
| `ValidationResult.IsDisposable` | `bool` | Whether the email domain belongs to a known disposable email provider. |
| `EmailValidator.ValidateWithSuggestion(string email)` | `ValidationResult` | Validates and suggests a correction if the domain is a typo of a common provider. |
| `EmailValidator.ValidateMany(IEnumerable<string> emails)` | `IReadOnlyList<ValidationResult>` | Validates multiple email addresses and returns results in order. |
| `EmailValidator.IsDisposable(string email)` | `bool` | Checks whether the email domain is a known disposable provider. |

## Development

```bash
dotnet build src/Philiprehberger.EmailValidator.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-email-validator)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-email-validator/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-email-validator/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
