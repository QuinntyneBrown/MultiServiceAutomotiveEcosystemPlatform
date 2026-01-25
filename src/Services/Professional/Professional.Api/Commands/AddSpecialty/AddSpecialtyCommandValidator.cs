using FluentValidation;

namespace Professional.Api.Commands.AddSpecialty;

public class AddSpecialtyCommandValidator : AbstractValidator<AddSpecialtyCommand>
{
    public AddSpecialtyCommandValidator()
    {
        RuleFor(x => x.ProfessionalId)
            .NotEmpty().WithMessage("Professional ID is required");

        RuleFor(x => x.SpecialtyId)
            .NotEmpty().WithMessage("Specialty ID is required");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative");
    }
}
