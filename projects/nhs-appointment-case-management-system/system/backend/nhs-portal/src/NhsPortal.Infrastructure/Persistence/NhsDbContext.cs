using Microsoft.EntityFrameworkCore;
using NhsPortal.Domain.Entities;
using NhsPortal.Infrastructure.Persistence.Entities;

namespace NhsPortal.Infrastructure.Persistence;

public class NhsDbContext : DbContext
{
    public NhsDbContext(DbContextOptions<NhsDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Clinician> Clinicians => Set<Clinician>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>().Property(x => x.Actor).HasMaxLength(200);
        modelBuilder.Entity<AuditLog>().Property(x => x.Action).HasMaxLength(50);
        modelBuilder.Entity<AuditLog>().Property(x => x.EntityType).HasMaxLength(200);
        modelBuilder.Entity<AuditLog>().Property(x => x.EntityId).HasMaxLength(200);
    }
}
