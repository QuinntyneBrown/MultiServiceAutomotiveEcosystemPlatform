using Shared.Contracts.Interfaces;

namespace Professional.Core.Entities;

public class ProfessionalSpecialty : IEntity
{
    public Guid Id { get; private set; }
    public Guid ProfessionalId { get; private set; }
    public Guid SpecialtyId { get; private set; }
    public string SpecialtyName { get; private set; } = string.Empty;
    public string? Certification { get; private set; }
    public bool IsVerified { get; private set; }
    public int YearsOfExperience { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ProfessionalSpecialty() { }

    public ProfessionalSpecialty(
        Guid professionalId,
        Guid specialtyId,
        string specialtyName,
        string? certification,
        int yearsOfExperience)
    {
        Id = Guid.NewGuid();
        ProfessionalId = professionalId;
        SpecialtyId = specialtyId;
        SpecialtyName = specialtyName;
        Certification = certification;
        YearsOfExperience = yearsOfExperience;
        CreatedAt = DateTime.UtcNow;
    }

    public void Verify()
    {
        IsVerified = true;
    }

    public void UpdateCertification(string? certification, int yearsOfExperience)
    {
        Certification = certification;
        YearsOfExperience = yearsOfExperience;
    }
}
