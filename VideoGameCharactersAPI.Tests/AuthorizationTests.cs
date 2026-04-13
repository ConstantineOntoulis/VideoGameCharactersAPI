using System.Net;
using System.Net.Http.Json;
using VideoGameCharactersAPI.Tests;
using Xunit;

namespace VideoGameCharactersAPI.Tests;

//Integration tests for authorization behavior
public class AuthorizationTests : IClassFixture<CustomWebApplicationFactory>
{
    //HttpClient acts as the test client that sends HTTP requests to the in-memory version of the API
    private readonly HttpClient _client;

    //xUnit provides the shared CustomWebApplicationFactory instance because this class uses IClassFixture
    public AuthorizationTests(CustomWebApplicationFactory factory)
    {
        //Create the client that will be used to call the API in tests
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateCharacter_WithoutToken_ReturnsUnauthorized()
    {
        //Arrange:Create a valid request body, but intentionally do not attach a token.The purpose of this test is to prove that authorization fails first
        var request = new
        {
            name = "Cloud",
            game = "Final Fantasy VII",
            role = "Hero"
        };

        //Act: Send the request to the protected create endpoint without authentication
        var response = await _client.PostAsJsonAsync("/api/VideoGameCharacters", request);

        //Assert: The API should reject the request with HTTP 401 Unauthorized because no bearer token was provided
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}