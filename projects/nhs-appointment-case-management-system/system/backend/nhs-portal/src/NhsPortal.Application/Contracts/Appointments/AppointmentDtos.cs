namespace NhsPortal.Application.Contracts.Appointments;

public sealed record CreateAppointmentRequest(
    int PatientId,
    int ClinicianId,
    DateTime ScheduledAtUtc,
    string? Notes
);

public sealed record AppointmentResponse(
    int Id,
    int PatientId,
    int ClinicianId,
    DateTime ScheduledAtUtc,
    string Status,
    string? Notes
);
