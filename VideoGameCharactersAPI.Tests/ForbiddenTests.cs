using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VideoGameCharactersAPI.Dtos;
using VideoGameCharactersAPI.Tests;
using Xunit;

namespace VideoGameCharacterApi.Tests;

//Integration tests for authorization failures caused by insufficient role permissions
public class ForbiddenTests : IClassFixture<CustomWebApplicationFactory>
{
    //HttpClient used to send requests to the in-memory test API host
    private readonly HttpClient _client;

    //xUnit provides the shared application factory to the constructor
    public ForbiddenTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    //Helper method:Log in as a normal User and return a valid JWT token
    //This token should authenticate successfully, but it should not be allowed to access endpoints that require the Admin role
    private async Task<string> GetUserTokenAsync()
    {
        var loginRequest = new LoginRequest
        {
            Username = "user",
            Password = "user123"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var loginBody = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        return loginBody!.Token;
    }

    [Fact]
    public async Task CreateCharacter_WithUserToken_ReturnsForbidden()
    {
        //Arrange:Obtain a valid token for a normal User account
        var token = await GetUserTokenAsync();

        //Attach the valid bearer token to the request
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        //Build a valid request body.The body itself is valid, so if the request fails,the reason should be authorization rather than validation
        var request = new
        {
            name = "Aerith",
            game = "Final Fantasy VII",
            role = "Hero"
        };

        //Act: Send the request to the protected create endpoint
        var response = await _client.PostAsJsonAsync("/api/VideoGameCharacters", request);

        //Assert:The request should fail with HTTP 403 Forbidden because the token belongs to a User rather than an Admin
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}