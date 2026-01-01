using System.Text.Json;
using NhsPortal.Application.Auditing;
using NhsPortal.Infrastructure.Persistence;
using NhsPortal.Infrastructure.Persistence.Entities;

namespace NhsPortal.Infrastructure.Auditing;

public sealed class EfAuditWriter : IAuditWriter
{
    private readonly NhsDbContext _db;

    public EfAuditWriter(NhsDbContext db) => _db = db;

    public async Task WriteAsync(
        string action,
        string entityType,
        string entityId,
        string actor,
        string? correlationId,
        object detail,
        CancellationToken ct)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Actor = actor,
            CorrelationId = correlationId ?? "",
            DetailJson = JsonSerializer.Serialize(detail)
        });

        await _db.SaveChangesAsync(ct);
    }
}
