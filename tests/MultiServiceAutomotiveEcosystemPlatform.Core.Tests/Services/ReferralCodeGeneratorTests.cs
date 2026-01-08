using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Services;

public class ReferralCodeGeneratorTests
{
    private readonly ReferralCodeGenerator _generator;

    public ReferralCodeGeneratorTests()
    {
        _generator = new ReferralCodeGenerator();
    }

    [Fact]
    public void GenerateCode_WithoutPrefix_ReturnsValidCode()
    {
        // Act
        var code = _generator.GenerateCode();

        // Assert
        Assert.NotNull(code);
        Assert.Equal(8, code.Length);
        Assert.True(_generator.IsValidCodeFormat(code));
    }

    [Fact]
    public void GenerateCode_WithPrefix_IncludesPrefix()
    {
        // Act
        var code = _generator.GenerateCode("JOHN");

        // Assert
        Assert.NotNull(code);
        Assert.StartsWith("JOHN", code);
        Assert.Equal(8, code.Length);
    }

    [Fact]
    public void GenerateCode_WithLongPrefix_TruncatesPrefix()
    {
        // Act
        var code = _generator.GenerateCode("VERYLONGNAME");

        // Assert
        Assert.NotNull(code);
        Assert.Equal(8, code.Length);
        Assert.StartsWith("VERY", code);
    }

    [Fact]
    public void GenerateCode_WithSpecialCharacters_SanitizesPrefix()
    {
        // Act
        var code = _generator.GenerateCode("John's");

        // Assert
        Assert.NotNull(code);
        Assert.True(code.All(c => char.IsLetterOrDigit(c)));
    }

    [Fact]
    public void GenerateCode_GeneratesUniqueCodesOnMultipleCalls()
    {
        // Act
        var codes = Enumerable.Range(0, 100)
            .Select(_ => _generator.GenerateCode())
            .ToHashSet();

        // Assert
        Assert.Equal(100, codes.Count);
    }

    [Fact]
    public void GenerateDiscountCode_ReturnsValidCode()
    {
        // Act
        var code = _generator.GenerateDiscountCode();

        // Assert
        Assert.NotNull(code);
        Assert.Equal(10, code.Length);
        Assert.StartsWith("DISC", code);
    }

    [Fact]
    public void GenerateDiscountCode_GeneratesUniqueCodesOnMultipleCalls()
    {
        // Act
        var codes = Enumerable.Range(0, 100)
            .Select(_ => _generator.GenerateDiscountCode())
            .ToHashSet();

        // Assert
        Assert.Equal(100, codes.Count);
    }

    [Theory]
    [InlineData("ABCD1234", true)]
    [InlineData("JOHN7K2M", true)]
    [InlineData("XYZ123456789", true)]
    [InlineData("ABC", false)] // Too short
    [InlineData("ABCDEFGHIJKLMNOP", false)] // Too long
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("abc123", false)] // Contains lowercase (not in allowed chars)
    [InlineData("ABCD!@#$", false)] // Contains special characters
    public void IsValidCodeFormat_ValidatesCorrectly(string code, bool expectedResult)
    {
        // Act
        var result = _generator.IsValidCodeFormat(code);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GenerateCode_DoesNotContainAmbiguousCharacters()
    {
        // Ambiguous characters: 0, O, I, 1, L
        var ambiguousChars = new[] { '0', 'O', 'I', '1', 'L' };

        // Generate many codes
        for (int i = 0; i < 100; i++)
        {
            var code = _generator.GenerateCode();
            Assert.DoesNotContain(code, c => ambiguousChars.Contains(c));
        }
    }

    [Fact]
    public void GenerateDiscountCode_DoesNotContainAmbiguousCharacters()
    {
        var ambiguousChars = new[] { '0', 'O', 'I', '1', 'L' };

        for (int i = 0; i < 100; i++)
        {
            var code = _generator.GenerateDiscountCode();
            Assert.DoesNotContain(code, c => ambiguousChars.Contains(c));
        }
    }
}
