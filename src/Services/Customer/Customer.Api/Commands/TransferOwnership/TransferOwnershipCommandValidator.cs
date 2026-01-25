using FluentValidation;

namespace Customer.Api.Commands.TransferOwnership;

public class TransferOwnershipCommandValidator : AbstractValidator<TransferOwnershipCommand>
{
    public TransferOwnershipCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.NewOwnerId)
            .NotEmpty().WithMessage("New owner ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required for ownership transfer")
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters");
    }
}
