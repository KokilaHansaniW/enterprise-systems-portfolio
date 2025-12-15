using Microsoft.EntityFrameworkCore;
using NhsPortal.Domain.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<NhsDbContext>(options =>
options.UseInMemoryDatabase("NhsPortal"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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

app.Run();
