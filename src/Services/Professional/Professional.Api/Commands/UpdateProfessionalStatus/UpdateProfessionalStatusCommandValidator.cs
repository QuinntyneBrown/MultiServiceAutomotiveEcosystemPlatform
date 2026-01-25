using FluentValidation;

namespace Professional.Api.Commands.UpdateProfessionalStatus;

public class UpdateProfessionalStatusCommandValidator : AbstractValidator<UpdateProfessionalStatusCommand>
{
    public UpdateProfessionalStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Professional ID is required");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value");
    }
}
