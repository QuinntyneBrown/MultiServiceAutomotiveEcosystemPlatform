namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

public class ProfessionalSpecialty
{
    public Guid ProfessionalSpecialtyId { get; private set; }
    public Guid ProfessionalId { get; private set; }
    public Guid? SpecialtyId { get; private set; }
    public string? CustomName { get; private set; }
    public string? Description { get; private set; }
    public int? YearsExperience { get; private set; }
    public string? CertificationName { get; private set; }
    public string? CertificationIssuer { get; private set; }
    public DateTime? CertificationDate { get; private set; }
    public DateTime? CertificationExpiry { get; private set; }
    public string? CertificationDocumentUrl { get; private set; }
    public bool Verified { get; private set; }
    public DateTime? VerifiedAt { get; private set; }
    public Guid? VerifiedBy { get; private set; }
    public int DisplayOrder { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation
    public Professional? Professional { get; private set; }
    public SpecialtyCatalog? SpecialtyCatalog { get; private set; }

    private ProfessionalSpecialty() { }

    public ProfessionalSpecialty(
        Guid professionalId,
        Guid? specialtyId = null,
        string? customName = null,
        string? description = null,
        int? yearsExperience = null,
        int displayOrder = 0)
    {
        if (specialtyId == null && string.IsNullOrWhiteSpace(customName))
            throw new ArgumentException("Either specialty ID or custom name must be provided.");

        ProfessionalSpecialtyId = Guid.NewGuid();
        ProfessionalId = professionalId;
        SpecialtyId = specialtyId;
        CustomName = customName?.Trim();
        Description = description?.Trim();
        YearsExperience = yearsExperience;
        DisplayOrder = displayOrder;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string? description, int? yearsExperience)
    {
        Description = description?.Trim();
        YearsExperience = yearsExperience;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCertification(
        string? certificationName,
        string? certificationIssuer,
        DateTime? certificationDate,
        DateTime? certificationExpiry,
        string? certificationDocumentUrl)
    {
        CertificationName = certificationName?.Trim();
        CertificationIssuer = certificationIssuer?.Trim();
        CertificationDate = certificationDate;
        CertificationExpiry = certificationExpiry;
        CertificationDocumentUrl = certificationDocumentUrl?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDisplayOrder(int displayOrder)
    {
        DisplayOrder = displayOrder;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Verify(Guid verifiedBy)
    {
        Verified = true;
        VerifiedAt = DateTime.UtcNow;
        VerifiedBy = verifiedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unverify()
    {
        Verified = false;
        VerifiedAt = null;
        VerifiedBy = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsCertificationExpired => CertificationExpiry.HasValue && CertificationExpiry.Value < DateTime.UtcNow;
}
