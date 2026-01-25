using FluentValidation;

namespace Referral.Api.Commands.CreateCustomerReferral;

public class CreateCustomerReferralCommandValidator : AbstractValidator<CreateCustomerReferralCommand>
{
    public CreateCustomerReferralCommandValidator()
    {
        RuleFor(x => x.ReferringCustomerId)
            .NotEmpty().WithMessage("Referring customer ID is required");

        RuleFor(x => x.ReferredEmail)
            .NotEmpty().WithMessage("Referred email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters");

        RuleFor(x => x.ReferredPhone)
            .MaximumLength(50).WithMessage("Phone must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferredPhone));

        RuleFor(x => x.ReferredFirstName)
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferredFirstName));

        RuleFor(x => x.ReferredLastName)
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferredLastName));

        RuleFor(x => x.ReferralSource)
            .MaximumLength(100).WithMessage("Referral source must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferralSource));

        RuleFor(x => x.ReferralCode)
            .MaximumLength(50).WithMessage("Referral code must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.ReferralCode));

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
