namespace Referral.Infrastructure.Data;

public class CustomerCache
{
    public Guid CustomerId { get; set; }
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}

public class ProfessionalCache
{
    public Guid ProfessionalId { get; set; }
    public Guid TenantId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}
