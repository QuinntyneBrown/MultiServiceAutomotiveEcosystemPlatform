using FluentValidation;

namespace Customer.Api.Commands.UpdateCustomerStatus;

public class UpdateCustomerStatusCommandValidator : AbstractValidator<UpdateCustomerStatusCommand>
{
    public UpdateCustomerStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value");
    }
}
