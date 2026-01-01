namespace NhsPortal.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public int ClinicianId { get; set; }

    // Use UTC consistently
    public DateTime ScheduledAtUtc { get; set; }

    // Status controlled by domain method
    public string Status { get; private set; } = AppointmentStatus.Scheduled;

    public string? Notes { get; set; }

    public void SetStatus(string newStatus)
    {
        if (!AppointmentStatus.Allowed.Contains(newStatus))
            throw new ArgumentException("Invalid appointment status.", nameof(newStatus));

        Status = newStatus;
    }
}

public static class AppointmentStatus
{
    public const string Scheduled = "Scheduled";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";

    public static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        Scheduled, Completed, Cancelled
    };
}