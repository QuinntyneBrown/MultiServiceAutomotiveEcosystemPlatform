namespace Shared.Infrastructure.Persistence;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int RetryCount { get; set; }
    public string? Error { get; set; }

    public static OutboxMessage Create<T>(T message) where T : class
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(T).AssemblyQualifiedName!,
            Content = System.Text.Json.JsonSerializer.Serialize(message),
            CreatedAt = DateTime.UtcNow
        };
    }
}
