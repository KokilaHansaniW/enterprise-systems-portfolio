using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NhsPortal.Application.Appointments;
using NhsPortal.Application.Contracts.Appointments;
using NhsPortal.Application.Auditing;
using NhsPortal.Domain.Entities;
using NhsPortal.Infrastructure.Auditing;
using NhsPortal.Infrastructure.Persistence;
using NhsPortal.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Swagger (with JWT support in UI)
// --------------------
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NHS Portal API",
        Version = "v1",
        Description = "Appointment & Case Management (portfolio build)"
    });

    // Add Bearer auth so you can test secured endpoints in Swagger
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}"
    });



});



// --------------------
// Database (SQLite for portfolio reliability)
// --------------------
var conn = builder.Configuration.GetConnectionString("NhsPortalDb");
if (string.IsNullOrWhiteSpace(conn))
{
    // Defensive fallback - prevents startup crash if config is missing
    conn = "Data Source=nhsportal.db";
}

builder.Services.AddDbContext<NhsDbContext>(opt => opt.UseSqlite(conn));

// --------------------
// Validation + Application services
// --------------------
builder.Services.AddValidatorsFromAssemblyContaining<CreateAppointmentValidator>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<NhsPortal.Application.Contracts.Appointments.IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAuditWriter, EfAuditWriter>();

// --------------------
// ProblemDetails (consistent errors)
// --------------------
builder.Services.AddProblemDetails();

// --------------------
// JWT Auth + Policies
// --------------------
var auth = builder.Configuration.GetSection("Auth");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = auth["Issuer"],
            ValidAudience = auth["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(auth["SigningKey"] ?? "DEV_ONLY_FALLBACK_KEY_CHANGE_ME")
            ),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
    options.AddPolicy("ClinicianOrAdmin", p => p.RequireRole("Clinician", "Admin"));
});

var app = builder.Build();

// Put exception handling early
app.UseExceptionHandler();

// Swagger: enable for portfolio (dev-friendly)
// app.UseSwagger();
// app.UseSwaggerUI();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NHS Portal API v1");

    // keep auth across refresh
    c.ConfigObject.PersistAuthorization = true;

    // Single-line interceptor to avoid JSON.parse errors
    c.UseRequestInterceptor("function (req) { var t = window.localStorage.getItem('nhsportal_token'); if (t) { req.headers['Authorization'] = t; } return req; }");
});



// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// --------------------
// Health (for CI/tests + ops)
// --------------------
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

// --------------------
// Correlation ID helper (light governance signal)
// --------------------
static string CorrelationId(HttpContext ctx)
{
    const string header = "X-Correlation-Id";
    if (!ctx.Request.Headers.TryGetValue(header, out var cid) || string.IsNullOrWhiteSpace(cid))
        cid = Guid.NewGuid().ToString("N");

    ctx.Response.Headers[header] = cid.ToString();
    return cid.ToString();
}

// --------------------
// DEV token issuance (so you can use Swagger today)
// NOTE: In real NHS, token issuance comes from Entra ID / identity provider.
// --------------------
app.MapPost("/auth/dev-token", () =>
{
    var issuer = auth["Issuer"] ?? "NhsPortal";
    var audience = auth["Audience"] ?? "NhsPortalUsers";
    var signingKey = auth["SigningKey"] ?? "DEV_ONLY_FALLBACK_KEY_CHANGE_ME";

    var claims = new List<Claim>
    {
        new(ClaimTypes.Name, "portfolio.user"),
        // Give both roles so you can test AdminOnly + ClinicianOrAdmin endpoints in Swagger
        new(ClaimTypes.Role, "Clinician"),
        new(ClaimTypes.Role, "Admin")
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds
    );

    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new
    {
        token_type = "Bearer",
        access_token = jwt,
        expires_in_seconds = 7200
    });
});

// --------------------
// Database migrate + seed (so create/list works immediately)
// --------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NhsDbContext>();

    // Migrate if migrations exist; fallback to EnsureCreated for demo resilience
    try
    {
        db.Database.Migrate();
    }
    catch
    {
        db.Database.EnsureCreated();
    }

    if (!db.Patients.Any())
    {
        db.Patients.Add(new Patient { FullName = "Demo Patient", NhsNumber = "9999999999" });
        db.SaveChanges();
    }

    if (!db.Clinicians.Any())
    {
        db.Clinicians.Add(new Clinician { Name = "Dr Demo", Specialty = "General Practice" });
        db.SaveChanges();
    }
}

// --------------------
// Appointment endpoints
// GETs are open; WRITE paths require roles (NHS-style governance)
// --------------------

// Create (secured)
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
    })
    .RequireAuthorization("ClinicianOrAdmin");

// List (open)
app.MapGet("/appointments", async (IAppointmentService service, CancellationToken ct)
    => Results.Ok(await service.ListAsync(ct)));

// Get one (open)
app.MapGet("/appointments/{id:int}", async (int id, IAppointmentService service, CancellationToken ct)
    => (await service.GetAsync(id, ct)) is { } a ? Results.Ok(a) : Results.NotFound());

// Update status (secured)
app.MapPatch("/appointments/{id:int}/status", async (
        int id,
        string status,
        IAppointmentService service,
        HttpContext ctx,
        CancellationToken ct) =>
    {
        var actor = ctx.User.Identity?.Name ?? "anonymous";
        var updated = await service.UpdateStatusAsync(id, status, actor, CorrelationId(ctx), ct);
        return updated is null ? Results.NotFound() : Results.Ok(updated);
    })
    .RequireAuthorization("ClinicianOrAdmin");

// Delete (secured)
app.MapDelete("/appointments/{id:int}", async (int id, IAppointmentService service, HttpContext ctx, CancellationToken ct) =>
    {
        var actor = ctx.User.Identity?.Name ?? "anonymous";
        var ok = await service.DeleteAsync(id, actor, CorrelationId(ctx), ct);
        return ok ? Results.NoContent() : Results.NotFound();
    })
    .RequireAuthorization("AdminOnly");

app.Run();
