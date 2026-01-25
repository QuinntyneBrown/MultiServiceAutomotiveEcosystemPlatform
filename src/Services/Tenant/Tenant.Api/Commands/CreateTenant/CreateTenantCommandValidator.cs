using FluentValidation;

namespace Tenant.Api.Commands.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.DisplayName)
            .MaximumLength(200).WithMessage("Display name must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.DisplayName));

        RuleFor(x => x.LogoUrl)
            .MaximumLength(500).WithMessage("Logo URL must not exceed 500 characters")
            .Must(BeAValidUrl).WithMessage("Logo URL must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl));

        RuleFor(x => x.PrimaryColor)
            .MaximumLength(50).WithMessage("Primary color must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.PrimaryColor));
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
