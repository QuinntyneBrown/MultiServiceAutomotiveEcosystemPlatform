namespace Referral.Core.Services;

public interface IReferralCodeGenerator
{
    string Generate(int length = 8);
}

public class ReferralCodeGenerator : IReferralCodeGenerator
{
    private const string Characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Excludes ambiguous characters
    private readonly Random _random = new();

    public string Generate(int length = 8)
    {
        var code = new char[length];
        for (int i = 0; i < length; i++)
        {
            code[i] = Characters[_random.Next(Characters.Length)];
        }
        return new string(code);
    }
}
