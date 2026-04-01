using Philiprehberger.EmailValidator;
using Xunit;

namespace Philiprehberger.EmailValidator.Tests;

public class DisposableEmailTests
{
    [Theory]
    [InlineData("user@mailinator.com")]
    [InlineData("user@guerrillamail.com")]
    [InlineData("user@tempmail.com")]
    [InlineData("user@yopmail.com")]
    [InlineData("user@trashmail.com")]
    public void IsDisposable_DisposableDomain_ReturnsTrue(string email)
    {
        Assert.True(EmailValidator.IsDisposable(email));
    }

    [Theory]
    [InlineData("user@gmail.com")]
    [InlineData("user@outlook.com")]
    [InlineData("user@yahoo.com")]
    [InlineData("user@mycompany.com")]
    public void IsDisposable_LegitDomain_ReturnsFalse(string email)
    {
        Assert.False(EmailValidator.IsDisposable(email));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-an-email")]
    public void IsDisposable_InvalidInput_ReturnsFalse(string email)
    {
        Assert.False(EmailValidator.IsDisposable(email));
    }

    [Fact]
    public void IsDisposable_CaseInsensitive_ReturnsTrue()
    {
        Assert.True(EmailValidator.IsDisposable("user@MAILINATOR.COM"));
    }
}
