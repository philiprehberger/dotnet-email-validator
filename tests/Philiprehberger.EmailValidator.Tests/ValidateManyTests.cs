using Philiprehberger.EmailValidator;
using Xunit;

namespace Philiprehberger.EmailValidator.Tests;

public class ValidateManyTests
{
    [Fact]
    public void ValidateMany_MixedEmails_ReturnsCorrectResults()
    {
        var emails = new[]
        {
            "user@example.com",
            "invalid",
            "another@test.org",
            "",
            "bad@.com"
        };

        var results = EmailValidator.ValidateMany(emails);

        Assert.Equal(5, results.Count);
        Assert.True(results[0].IsValid);
        Assert.False(results[1].IsValid);
        Assert.True(results[2].IsValid);
        Assert.False(results[3].IsValid);
        Assert.False(results[4].IsValid);
    }

    [Fact]
    public void ValidateMany_AllValid_ReturnsAllValid()
    {
        var emails = new[] { "a@b.com", "c@d.org", "e@f.net" };

        var results = EmailValidator.ValidateMany(emails);

        Assert.Equal(3, results.Count);
        Assert.All(results, r => Assert.True(r.IsValid));
    }

    [Fact]
    public void ValidateMany_EmptyList_ReturnsEmptyList()
    {
        var results = EmailValidator.ValidateMany(Array.Empty<string>());

        Assert.Empty(results);
    }

    [Fact]
    public void ValidateMany_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => EmailValidator.ValidateMany(null!));
    }

    [Fact]
    public void ValidateMany_PreservesOrder()
    {
        var emails = new[] { "first@example.com", "second@example.com", "third@example.com" };

        var results = EmailValidator.ValidateMany(emails);

        Assert.Equal("first@example.com", results[0].NormalizedAddress);
        Assert.Equal("second@example.com", results[1].NormalizedAddress);
        Assert.Equal("third@example.com", results[2].NormalizedAddress);
    }
}
