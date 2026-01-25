using FluentValidation;

namespace Referral.Api.Commands.CreateProfessionalReferral;

public class CreateProfessionalReferralCommandValidator : AbstractValidator<CreateProfessionalReferralCommand>
{
    public CreateProfessionalReferralCommandValidator()
    {
        RuleFor(x => x.ReferringProfessionalId)
            .NotEmpty().WithMessage("Referring professional ID is required");

        RuleFor(x => x.ReferredBusinessName)
            .NotEmpty().WithMessage("Referred business name is required")
            .MaximumLength(200).WithMessage("Business name cannot exceed 200 characters");

        RuleFor(x => x.ReferredEmail)
            .NotEmpty().WithMessage("Referred email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters");

        RuleFor(x => x.ReferredPhone)
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferredPhone));

        RuleFor(x => x.ContactPersonName)
            .MaximumLength(100).WithMessage("Contact person name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactPersonName));

        RuleFor(x => x.ReferralSource)
            .MaximumLength(100).WithMessage("Referral source cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferralSource));

        RuleFor(x => x.ReferralCode)
            .MaximumLength(50).WithMessage("Referral code cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferralCode));

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
