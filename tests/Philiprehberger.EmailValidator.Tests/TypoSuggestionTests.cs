using Philiprehberger.EmailValidator;
using Xunit;

namespace Philiprehberger.EmailValidator.Tests;

public class TypoSuggestionTests
{
    [Theory]
    [InlineData("user@gmial.com", "user@gmail.com")]
    [InlineData("user@gmal.com", "user@gmail.com")]
    [InlineData("user@yaho.com", "user@yahoo.com")]
    [InlineData("user@hotmial.com", "user@hotmail.com")]
    [InlineData("user@outlok.com", "user@outlook.com")]
    [InlineData("user@protonmal.com", "user@protonmail.com")]
    public void ValidateWithSuggestion_TypoDomain_ReturnsSuggestion(string email, string expected)
    {
        var result = EmailValidator.ValidateWithSuggestion(email);

        Assert.Equal(expected, result.Suggestion);
    }

    [Fact]
    public void ValidateWithSuggestion_CorrectDomain_ReturnsNullSuggestion()
    {
        var result = EmailValidator.ValidateWithSuggestion("user@gmail.com");

        Assert.True(result.IsValid);
        Assert.Null(result.Suggestion);
    }

    [Fact]
    public void ValidateWithSuggestion_UnknownDomain_ReturnsNullSuggestion()
    {
        var result = EmailValidator.ValidateWithSuggestion("user@mycompany.com");

        Assert.True(result.IsValid);
        Assert.Null(result.Suggestion);
    }

    [Fact]
    public void ValidateWithSuggestion_InvalidEmail_StillReturnsSuggestionIfDomainIsTypo()
    {
        var result = EmailValidator.ValidateWithSuggestion("user@gmial.com");

        Assert.NotNull(result.Suggestion);
        Assert.Equal("user@gmail.com", result.Suggestion);
    }
}
