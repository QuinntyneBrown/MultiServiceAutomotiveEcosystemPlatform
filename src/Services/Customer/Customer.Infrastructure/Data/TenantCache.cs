namespace Customer.Infrastructure.Data;

public class TenantCache
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}
