namespace NhsPortal.Application.Auditing;

public interface IAuditWriter
{
    Task WriteAsync(
        string action,
        string entityType,
        string entityId,
        string actor,
        string correlationId,
        object detail,
        CancellationToken ct);
}
