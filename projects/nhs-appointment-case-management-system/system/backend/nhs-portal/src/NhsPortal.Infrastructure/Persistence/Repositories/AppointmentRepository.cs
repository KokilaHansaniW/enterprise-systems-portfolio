using Microsoft.EntityFrameworkCore;
using NhsPortal.Application.Contracts.Appointments;
using NhsPortal.Domain.Entities;

namespace NhsPortal.Infrastructure.Persistence.Repositories;

public sealed class AppointmentRepository : IAppointmentRepository
{
    private readonly NhsDbContext _db;

    public AppointmentRepository(NhsDbContext db) => _db = db;

    public async Task AddAsync(Appointment appointment, CancellationToken ct)
        => await _db.Appointments.AddAsync(appointment, ct);

    public async Task<IReadOnlyList<Appointment>> ListAsync(CancellationToken ct)
        => await _db.Appointments.AsNoTracking().ToListAsync(ct);

    public async Task<Appointment?> GetAsync(int id, CancellationToken ct)
        => await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task DeleteAsync(Appointment appointment, CancellationToken ct)
    {
        _db.Appointments.Remove(appointment);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
