using FluentValidation;

namespace Referral.Api.Commands.GenerateReferralCode;

public class GenerateReferralCodeCommandValidator : AbstractValidator<GenerateReferralCodeCommand>
{
    public GenerateReferralCodeCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => x.CustomerId.HasValue || x.ProfessionalId.HasValue)
            .WithMessage("Either CustomerId or ProfessionalId must be provided");

        RuleFor(x => x.CampaignName)
            .MaximumLength(100).WithMessage("Campaign name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.CampaignName));

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future")
            .When(x => x.ExpiresAt.HasValue);

        RuleFor(x => x.MaxUses)
            .GreaterThan(0).WithMessage("Max uses must be greater than 0")
            .When(x => x.MaxUses.HasValue);
    }
}
