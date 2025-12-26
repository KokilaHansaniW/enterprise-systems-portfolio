namespace NhsPortal.Application.Contracts.Appointments;

public record CreateAppointmentRequest(
    int PatientId,
    int ClinicianId,
    DateTime ScheduledAtUtc,
    string? Notes
);

public record AppointmentResponse(
    int Id,
    int PatientId,
    int ClinicianId,
    DateTime ScheduledAtUtc,
    string Status,
    string? Notes
);
