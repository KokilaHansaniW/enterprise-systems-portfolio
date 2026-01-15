using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using NhsPortal.Application.Contracts.Appointments;

namespace NhsPortal.ApiTests;

public class AppointmentTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AppointmentTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task Create_Then_List_ReturnsAppointment()
    {
        // ✅ Add auth BEFORE calling secured endpoints
        var token = await GetDevTokenAsync(_client);
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

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

    private static async Task<string> GetDevTokenAsync(HttpClient client)
    {
        var resp = await client.PostAsync("/auth/dev-token", content: null);
        resp.EnsureSuccessStatusCode();

        var json = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        return doc.RootElement.GetProperty("access_token").GetString()!;
    }
}