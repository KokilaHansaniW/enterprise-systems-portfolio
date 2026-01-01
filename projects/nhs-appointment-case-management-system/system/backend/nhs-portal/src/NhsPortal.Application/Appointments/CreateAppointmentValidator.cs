using FluentValidation;
using NhsPortal.Application.Contracts.Appointments;

namespace NhsPortal.Application.Appointments;

public sealed class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.ClinicianId).GreaterThan(0);

        RuleFor(x => x.ScheduledAtUtc)
            .Must(dt => dt > DateTime.UtcNow.AddMinutes(10))
            .WithMessage("Appointment must be at least 10 minutes in the future.");

        RuleFor(x => x.Notes).MaximumLength(2000);
    }
}
