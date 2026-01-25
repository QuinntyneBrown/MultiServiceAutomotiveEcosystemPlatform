using FluentValidation;

namespace Professional.Api.Commands.CreateProfessional;

public class CreateProfessionalCommandValidator : AbstractValidator<CreateProfessionalCommand>
{
    public CreateProfessionalCommandValidator()
    {
        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("Business name is required")
            .MaximumLength(200).WithMessage("Business name must not exceed 200 characters");

        RuleFor(x => x.BusinessDescription)
            .MaximumLength(2000).WithMessage("Business description must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.BusinessDescription));

        RuleFor(x => x.BusinessPhone)
            .MaximumLength(50).WithMessage("Business phone must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.BusinessPhone));

        RuleFor(x => x.BusinessEmail)
            .EmailAddress().WithMessage("Invalid business email format")
            .MaximumLength(256).WithMessage("Business email must not exceed 256 characters")
            .When(x => !string.IsNullOrEmpty(x.BusinessEmail));

        RuleFor(x => x.Website)
            .MaximumLength(500).WithMessage("Website must not exceed 500 characters")
            .Must(BeAValidUrl).WithMessage("Website must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(50).WithMessage("Phone must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.ReferralCommissionRate)
            .InclusiveBetween(0, 100).WithMessage("Referral commission rate must be between 0 and 100")
            .When(x => x.ReferralCommissionRate.HasValue);
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
