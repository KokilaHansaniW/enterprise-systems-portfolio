using NhsPortal.Domain.Entities;

namespace NhsPortal.Application.Appointments;

public interface IAppointmentRepository
{
    Task<Appointment> AddAsync(Appointment appointment, CancellationToken ct);
    Task<IReadOnlyList<Appointment>> ListAsync(CancellationToken ct);
    Task<Appointment?> GetAsync(int id, CancellationToken ct);
    Task DeleteAsync(Appointment appointment, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
