using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VideoGameCharactersAPI.Dtos;
using VideoGameCharactersAPI.Tests;
using Xunit;

namespace VideoGameCharactersAPI.Tests;

//Integration tests for request validation behavior on protected endpoints
public class ValidationTests : IClassFixture<CustomWebApplicationFactory>
{
    //HttpClient used to send requests to the in-memory test API host
    private readonly HttpClient _client;
    //The factory boots the API in the test environment, and CreateClient() gives this class an HttpClient connected to that test host
    public ValidationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    //Helper method: Logs in as Admin and returns a valid JWT token
    private async Task<string> GetAdminTokenAsync()
    {
        //Build a valid login request using the demo admin credentials
        var loginRequest = new LoginRequest
        {
            Username = "admin",
            Password = "admin123"
        };

        //Send the login request to the auth endpoint
        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        //Ensure the login request succeeded before continuing
        loginResponse.EnsureSuccessStatusCode();

        //Deserialize the response body into the expected DTO
        var loginBody = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        //Return the token so it can be attached to later requests
        return loginBody!.Token;
    }

    [Fact]
    public async Task CreateCharacter_WithInvalidBody_ReturnsBadRequest()
    {
        //Arrange:Authenticate as Admin so the request reaches validation
        var token = await GetAdminTokenAsync();

        //Attach the bearer token to the request header
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        //Arrange an invalid create request body
        var invalidRequest = new
        {  //Arrange: These values are intentionally empty so they violate the validation rules defined in the request DTO
            name = "",
            game = "",
            role = ""
        };

        //Act:Send the invalid request to the create endpoint as JSON
        var response = await _client.PostAsJsonAsync("/api/VideoGameCharacters", invalidRequest);

        //Assert:Confirm that the API rejects the invalid request with HTTP 400 Bad Request
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
