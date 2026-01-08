namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public interface IReferralCodeGenerator
{
    string GenerateCode(string? namePrefix = null);
    string GenerateDiscountCode();
    bool IsValidCodeFormat(string code);
}
