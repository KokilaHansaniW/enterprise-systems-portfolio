using Microsoft.AspNetCore.Mvc.Testing;

namespace NhsPortal.ApiTests;

public class HealthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public HealthTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Health_Returns200()
    {
        var res = await _client.GetAsync("/health");
        Assert.True(res.IsSuccessStatusCode);
    }
}
