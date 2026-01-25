using FluentValidation;

namespace Tenant.Api.Commands.DeleteTenant;

public class DeleteTenantCommandValidator : AbstractValidator<DeleteTenantCommand>
{
    public DeleteTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tenant ID is required");
    }
}
