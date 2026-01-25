using FluentValidation;

namespace Customer.Api.Commands.AssignOwner;

public class AssignOwnerCommandValidator : AbstractValidator<AssignOwnerCommand>
{
    public AssignOwnerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.ProfessionalId)
            .NotEmpty().WithMessage("Professional ID is required");
    }
}
