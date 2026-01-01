using NhsPortal.Application.Auditing;
using NhsPortal.Application.Contracts.Appointments;
using NhsPortal.Domain.Entities;

namespace NhsPortal.Application.Appointments;

public sealed class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repo;
    private readonly IAuditWriter _audit;

    public AppointmentService(IAppointmentRepository repo, IAuditWriter audit)
    {
        _repo = repo;
        _audit = audit;
    }

    public async Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request, string actor, string correlationId, CancellationToken ct)
    {
        var appt = new Appointment
        {
            PatientId = request.PatientId,
            ClinicianId = request.ClinicianId,
            ScheduledAtUtc = request.ScheduledAtUtc,
            Notes = request.Notes
        };

        appt.SetStatus(AppointmentStatus.Scheduled);

        await _repo.AddAsync(appt, ct);
        await _repo.SaveChangesAsync(ct);

        await _audit.WriteAsync(
            action: "CREATE",
            entityType: "Appointment",
            entityId: appt.Id.ToString(),
            actor: actor,
            correlationId: correlationId,
            detail: new { appt.PatientId, appt.ClinicianId, appt.ScheduledAtUtc, appt.Status },
            ct: ct);

        return Map(appt);
    }

    public async Task<IReadOnlyList<AppointmentResponse>> ListAsync(CancellationToken ct)
        => (await _repo.ListAsync(ct)).Select(Map).ToList();

    public async Task<AppointmentResponse?> GetAsync(int id, CancellationToken ct)
        => (await _repo.GetAsync(id, ct)) is { } a ? Map(a) : null;

    public async Task<AppointmentResponse?> UpdateStatusAsync(int id, string status, string actor, string correlationId, CancellationToken ct)
    {
        var appt = await _repo.GetAsync(id, ct);
        if (appt is null) return null;

        appt.SetStatus(status);
        await _repo.SaveChangesAsync(ct);

        await _audit.WriteAsync("UPDATE_STATUS", "Appointment", appt.Id.ToString(), actor, correlationId, new { appt.Status }, ct);

        return Map(appt);
    }

    public async Task<bool> DeleteAsync(int id, string actor, string correlationId, CancellationToken ct)
    {
        var appt = await _repo.GetAsync(id, ct);
        if (appt is null) return false;

        await _repo.DeleteAsync(appt, ct);
        await _repo.SaveChangesAsync(ct);

        await _audit.WriteAsync("DELETE", "Appointment", id.ToString(), actor, correlationId, new { }, ct);

        return true;
    }

    private static AppointmentResponse Map(Appointment a)
        => new(a.Id, a.PatientId, a.ClinicianId, a.ScheduledAtUtc, a.Status, a.Notes);
}
