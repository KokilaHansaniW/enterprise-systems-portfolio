using Microsoft.EntityFrameworkCore;
using NhsPortal.Domain.Entities;

namespace NhsPortal.Domain.Persistence;

public class NhsDbContext : DbContext
{
    public NhsDbContext(DbContextOptions<NhsDbContext> options)
        : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Clinician> Clinicians => Set<Clinician>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
}
