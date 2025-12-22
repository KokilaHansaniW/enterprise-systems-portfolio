using Microsoft.EntityFrameworkCore;
using NhsPortal.Domain.Persistence;
using NhsPortal.Domain.Entities;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<NhsDbContext>(options =>
options.UseInMemoryDatabase("NhsPortal"));

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.MapGet("/", () =>
    Results.Ok(new
    {
        service = "NhsPortal.Api",
        status = "Running"
    })
);

app.MapGet("/health", () =>
    Results.Ok(new
    {
        status = "Healthy",
        timeUtc = DateTime.UtcNow
    })
);

// APPOINTMENT CRUD

// Create appointment
app.MapPost("/appoinments",async (NhsDbContext db, Appointment appt) =>
{
    if (appt.ScheduledAt < DateTime.UtcNow)
        return Results.BadRequest("Appointment must be in the future.");

    db.Appointments.Add(appt);
    await db.SaveChangesAsync();
    return Results.Created($"/appointments/{appt.Id}", appt);
});

// Get all appointments
app.MapGet("/appointments", async (NhsDbContext db) =>
    await db.Appointments.ToListAsync());

// Get one
app.MapGet("/appointments/{id:int}", async (NhsDbContext db, int id) =>
{
    var appt = await db.Appointments.FindAsync(id);
    return appt is null ? Results.NotFound() : Results.Ok(appt);
});

// Update
app.MapPut("/appointments/{id:int}", async (NhsDbContext db, int id, Appointment updated) =>
{
    var appt = await db.Appointments.FindAsync(id);
    if (appt is null) return Results.NotFound();

    appt.ScheduledAt = updated.ScheduledAt;
    appt.Status = updated.Status;

    await db.SaveChangesAsync();
    return Results.Ok(appt);
});

// Delete
app.MapDelete("/appointments/{id:int}", async (NhsDbContext db, int id) =>
{
    var appt = await db.Appointments.FindAsync(id);
    if (appt is null) return Results.NotFound();

    db.Appointments.Remove(appt);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();