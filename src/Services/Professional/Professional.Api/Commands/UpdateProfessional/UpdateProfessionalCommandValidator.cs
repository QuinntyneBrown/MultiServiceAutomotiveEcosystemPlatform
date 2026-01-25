using FluentValidation;

namespace Professional.Api.Commands.UpdateProfessional;

public class UpdateProfessionalCommandValidator : AbstractValidator<UpdateProfessionalCommand>
{
    public UpdateProfessionalCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Professional ID is required");

        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("Business name is required")
            .MaximumLength(200).WithMessage("Business name must not exceed 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");
    }
}
