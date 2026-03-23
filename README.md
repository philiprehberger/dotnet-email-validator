# Philiprehberger.EmailValidator

[![CI](https://github.com/philiprehberger/dotnet-email-validator/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-email-validator/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.EmailValidator.svg)](https://www.nuget.org/packages/Philiprehberger.EmailValidator)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-email-validator)](LICENSE)

RFC 5321/5322 compliant email address validation with international domain support and structured error messages.

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

## API

| Member | Returns | Description |
|--------|---------|-------------|
| `EmailValidator.IsValid(string email)` | `bool` | Returns whether the email address is valid. |
| `EmailValidator.Validate(string email)` | `ValidationResult` | Returns a detailed validation result with error and normalized address. |
| `ValidationResult.IsValid` | `bool` | Whether the email address is valid. |
| `ValidationResult.Error` | `string?` | Error message if invalid, null if valid. |
| `ValidationResult.NormalizedAddress` | `string?` | Lowercased, trimmed email if valid, null if invalid. |

## Development

```bash
dotnet build src/Philiprehberger.EmailValidator.csproj --configuration Release
```

## License

MIT
