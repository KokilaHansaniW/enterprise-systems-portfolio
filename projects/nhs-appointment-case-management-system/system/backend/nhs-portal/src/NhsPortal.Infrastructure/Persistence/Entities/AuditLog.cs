namespace NhsPortal.Infrastructure.Persistence.Entities;

public class AuditLog
{
    public long Id { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

    public string Actor { get; set; } = "unknown";
    public string Action { get; set; } = "";
    public string EntityType { get; set; } = "";
    public string EntityId { get; set; } = "";

    public string CorrelationId { get; set; } = "";
    public string? DetailJson { get; set; } // Ensure no PII stored here
}
