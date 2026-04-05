using System.Net;
using VideoGameCharactersAPI.Tests;
using Xunit;

namespace VideoGameCharactersAPI.Tests;

//Class uses the custom web application factory to boot the API in a test environment
public class SmokeTests : IClassFixture<CustomWebApplicationFactory>
{
    //HttpClient will act as the test client that sends HTTP requests to the in-memory test version of the API
    private readonly HttpClient _Client;
    //xUnit will create the CustomWebApplicationFactory once for this test class and pass it into the constructor
    public SmokeTests(CustomWebApplicationFactory factory)
    {
        _Client = factory.CreateClient(); //Create an HttpClient connected to the test host
    }
    [Fact] //a test method that xUnit should run
    public async Task App_Boots_And_Responds()
    {
        //Send a GET request to the root path of the application, only checking that the app can boot and respond to a request
        var response = await _Client.GetAsync("/");
        Assert.True( //Verify that a response object was actually returned
            response.StatusCode == HttpStatusCode.OK ||
            response.StatusCode == HttpStatusCode.NotFound);
    }
}