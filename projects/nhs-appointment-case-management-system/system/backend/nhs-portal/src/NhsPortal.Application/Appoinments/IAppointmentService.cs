namespace NhsPortal.Application.Contracts.Appointments;

public interface IAppointmentService
{
    Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request, string actor, CancellationToken ct);
    Task<IReadOnlyList<AppointmentResponse>> ListAsync(CancellationToken ct);
    Task<AppointmentResponse?> GetAsync(int id, CancellationToken ct);
    Task<AppointmentResponse?> UpdateStatusAsync(int id, string status, string actor, CancellationToken ct);
    Task<bool> DeleteAsync(int id, string actor, CancellationToken ct);
}
