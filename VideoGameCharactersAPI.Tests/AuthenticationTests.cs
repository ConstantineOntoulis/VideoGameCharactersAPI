using System.Net;
using System.Net.Http.Json;
using VideoGameCharactersAPI.Dtos;
using VideoGameCharactersAPI.Tests;
using Xunit;

namespace VideoGameCharactersAPI.Tests;

//Integration tests for authentication behavior
public class AuthenticationTests : IClassFixture<CustomWebApplicationFactory>
{
    //Shared HttpClient used to send HTTP requests to the in-memory API host
    private readonly HttpClient _client;
    //The constructor receives the shared application factory from xUnit, 
    //factory boots the API in the test environment, and CreateClient() gives this class an HttpClient connected to that test host

    public AuthenticationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidAdminCredentials_ReturnsOkAndToken()
    {
        //Arrange:Create a valid login request using the demo admin credentials
        //This object represents the JSON request body that will be sent to the API
        var loginRequest = new LoginRequest
        {
            Username = "admin",
            Password = "admin123"
        };

        //Act:Send the login request to the auth endpoint, API processes the credentials and return an HTTP response
        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        //Assert:First confirm that the login request succeeded with HTTP 200
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //Read the json response body as LoginResponse, lets the test inspect the returned token and role values
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

        //Confirm that the response body was returned and could be deserialized successfully
        Assert.NotNull(loginResponse);

        //Confirm that the token string exists and is not empty, successful login returns a usable JWT token
        Assert.False(string.IsNullOrWhiteSpace(loginResponse!.Token));

        //Confirm that the expected role is returned
        Assert.Equal("Admin", loginResponse.Role);
    }
}