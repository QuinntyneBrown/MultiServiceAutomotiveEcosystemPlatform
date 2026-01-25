using FluentValidation;
using Shared.Contracts.Enums;

namespace Tenant.Api.Commands.UpdateTenantStatus;

public class UpdateTenantStatusCommandValidator : AbstractValidator<UpdateTenantStatusCommand>
{
    public UpdateTenantStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tenant ID is required");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value");
    }
}
