namespace Referral.Core.Services;

public static class ReferralCodeGenerator
{
    private const string Characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Excludes ambiguous characters
    private static readonly Random Random = new();

    public static string Generate(int length = 8)
    {
        var code = new char[length];
        for (int i = 0; i < length; i++)
        {
            code[i] = Characters[Random.Next(Characters.Length)];
        }
        return new string(code);
    }
}
