using System.Security.Cryptography;
using System.Text;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public class ReferralCodeGenerator : IReferralCodeGenerator
{
    // Characters to use in codes - excluding ambiguous characters (0, O, I, 1, L)
    private const string AllowedCharacters = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
    private const int DefaultCodeLength = 8;
    private const int DiscountCodeLength = 10;

    private static readonly HashSet<string> BlacklistedCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        // Add any offensive combinations here
        "FUCK", "SHIT", "DAMN", "HELL", "CRAP", "ASS", "SEX"
    };

    public string GenerateCode(string? namePrefix = null)
    {
        string code;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            var prefix = GetSanitizedPrefix(namePrefix);
            var suffixLength = DefaultCodeLength - prefix.Length;
            var suffix = GenerateRandomString(suffixLength);
            code = prefix + suffix;
            attempts++;

            if (attempts >= maxAttempts)
            {
                // If we've tried too many times, just generate a fully random code
                code = GenerateRandomString(DefaultCodeLength);
                break;
            }
        }
        while (ContainsBlacklistedWord(code));

        return code;
    }

    public string GenerateDiscountCode()
    {
        string code;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            code = "DISC" + GenerateRandomString(DiscountCodeLength - 4);
            attempts++;

            if (attempts >= maxAttempts)
            {
                code = GenerateRandomString(DiscountCodeLength);
                break;
            }
        }
        while (ContainsBlacklistedWord(code));

        return code;
    }

    public bool IsValidCodeFormat(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        if (code.Length < 6 || code.Length > 12)
            return false;

        return code.All(c => AllowedCharacters.Contains(c));
    }

    private static string GetSanitizedPrefix(string? namePrefix)
    {
        if (string.IsNullOrWhiteSpace(namePrefix))
            return string.Empty;

        var sanitized = new StringBuilder();
        foreach (var c in namePrefix.ToUpperInvariant())
        {
            if (AllowedCharacters.Contains(c))
            {
                sanitized.Append(c);
            }
        }

        // Limit prefix to 4 characters to leave room for suffix
        var result = sanitized.ToString();
        return result.Length > 4 ? result[..4] : result;
    }

    private static string GenerateRandomString(int length)
    {
        if (length <= 0)
            return string.Empty;

        var bytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        var result = new StringBuilder(length);
        foreach (var b in bytes)
        {
            result.Append(AllowedCharacters[b % AllowedCharacters.Length]);
        }

        return result.ToString();
    }

    private static bool ContainsBlacklistedWord(string code)
    {
        var upperCode = code.ToUpperInvariant();
        return BlacklistedCodes.Any(blacklisted => upperCode.Contains(blacklisted));
    }
}
