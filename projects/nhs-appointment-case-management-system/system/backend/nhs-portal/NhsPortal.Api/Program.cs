using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

using NhsPortal.Application.Appointments; // or correct namespace you used
using NhsPortal.Application.Contracts.Appointments;
using NhsPortal.Application.Auditing;
using NhsPortal.Infrastructure.Persistence;
using NhsPortal.Infrastructure.Persistence.Repositories;
using NhsPortal.Infrastructure.Auditing;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB (use what you already configured: SQLite or SQL Server)
builder.Services.AddDbContext<NhsDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("NhsPortalDb")));

// Validation + services
builder.Services.AddValidatorsFromAssemblyContaining<CreateAppointmentValidator>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<NhsPortal.Application.Contracts.Appointments.IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAuditWriter, EfAuditWriter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Helper
static string CorrelationId(HttpContext ctx)
{
    const string header = "X-Correlation-Id";
    if (!ctx.Request.Headers.TryGetValue(header, out var cid) || string.IsNullOrWhiteSpace(cid))
        cid = Guid.NewGuid().ToString("N");

    ctx.Response.Headers[header] = cid.ToString();
    return cid.ToString();
}

// APPOINTMENT CRUD

// Create appointment
// Endpoints (service-based)
app.MapPost("/appointments", async (
    CreateAppointmentRequest req,
    IValidator<CreateAppointmentRequest> validator,
    IAppointmentService service,
    HttpContext ctx,
    CancellationToken ct) =>
{
    var validation = await validator.ValidateAsync(req, ct);
    if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

    var actor = ctx.User.Identity?.Name ?? "anonymous";
    var created = await service.CreateAsync(req, actor, CorrelationId(ctx), ct);
    return Results.Created($"/appointments/{created.Id}", created);
});


// Get all appointments
app.MapGet("/appointments", async (IAppointmentService service, CancellationToken ct)
    => Results.Ok(await service.ListAsync(ct)));


// Get one
app.MapGet("/appointments/{id:int}", async (int id, IAppointmentService service, CancellationToken ct)
    => (await service.GetAsync(id, ct)) is { } a ? Results.Ok(a) : Results.NotFound());

// update
app.MapPatch("/appointments/{id:int}/status", async (int id, string status, IAppointmentService service, HttpContext ctx, CancellationToken ct) =>
{
    var actor = ctx.User.Identity?.Name ?? "anonymous";
    var updated = await service.UpdateStatusAsync(id, status, actor, CorrelationId(ctx), ct);
    return updated is null ? Results.NotFound() : Results.Ok(updated);
});

// Delete
app.MapDelete("/appointments/{id:int}", async (int id, IAppointmentService service, HttpContext ctx, CancellationToken ct) =>
{
    var actor = ctx.User.Identity?.Name ?? "anonymous";
    var ok = await service.DeleteAsync(id, actor, CorrelationId(ctx), ct);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.Run();