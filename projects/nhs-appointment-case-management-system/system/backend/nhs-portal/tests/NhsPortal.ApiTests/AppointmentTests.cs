using System.Net.Http.Json;
using NhsPortal.Application.Contracts.Appointments;

namespace NhsPortal.ApiTests;

public class AppointmentTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    public AppointmentTests(CustomWebApplicationFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Create_Then_List_ReturnsAppointment()
    {
        var req = new CreateAppointmentRequest(
            PatientId: 1,
            ClinicianId: 1,
            ScheduledAtUtc: DateTime.UtcNow.AddHours(2),
            Notes: "Initial booking"
        );

        var create = await _client.PostAsJsonAsync("/appointments", req);
        Assert.Equal(System.Net.HttpStatusCode.Created, create.StatusCode);

        var list = await _client.GetFromJsonAsync<List<AppointmentResponse>>("/appointments");
        Assert.NotNull(list);
        Assert.True(list!.Count >= 1);
    }
}
