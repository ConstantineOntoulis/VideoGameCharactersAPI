using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VideoGameCharactersAPI.Dtos;
using VideoGameCharactersAPI.Tests;
using Xunit;

namespace VideoGameCharactersAPI.Tests;

//Integration tests for successful access to protected endpoints
public class SuccessEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SuccessEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    //Helper method:Log in as Admin and return a valid JWT token
    private async Task<string> GetAdminTokenAsync()
    {
        var loginRequest = new LoginRequest
        {
            Username = "admin",
            Password = "admin123"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var loginBody = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        return loginBody!.Token;
    }

    [Fact]
    public async Task CreateCharacter_WithAdminTokenAndValidBody_ReturnsCreated()
    {
        //Arrange:Obtain a valid admin token and attach it to the request
        var token = await GetAdminTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        //Build a valid request body.
        var request = new
        {
            name = "Barret",
            game = "Final Fantasy VII",
            role = "Hero"
        };

        //Act:Send the authenticated request to the protected create endpoint
        var response = await _client.PostAsJsonAsync("/api/VideoGameCharacters", request);

        //Assert:A valid authenticated admin request should succeed with HTTP 201 Created
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}