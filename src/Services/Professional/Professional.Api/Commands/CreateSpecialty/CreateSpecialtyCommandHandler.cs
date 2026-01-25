using MediatR;
using Professional.Core.Entities;
using Professional.Core.Interfaces;
using Professional.Infrastructure.Data;
using Shared.Contracts.DTOs;

namespace Professional.Api.Commands.CreateSpecialty;

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, SpecialtyCatalogDto>
{
    private readonly ISpecialtyCatalogRepository _repository;
    private readonly ProfessionalDbContext _context;
    private readonly ILogger<CreateSpecialtyCommandHandler> _logger;

    public CreateSpecialtyCommandHandler(
        ISpecialtyCatalogRepository repository,
        ProfessionalDbContext context,
        ILogger<CreateSpecialtyCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _logger = logger;
    }

    public async Task<SpecialtyCatalogDto> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = new SpecialtyCatalog(request.Name, request.Description, request.Category);

        await _repository.AddAsync(specialty, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created specialty {SpecialtyId} ({SpecialtyName})", specialty.Id, specialty.Name);

        return new SpecialtyCatalogDto
        {
            Id = specialty.Id,
            Name = specialty.Name,
            Description = specialty.Description,
            Category = specialty.Category,
            IsActive = specialty.IsActive
        };
    }
}
