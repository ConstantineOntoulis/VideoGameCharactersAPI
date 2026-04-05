using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

// This is the reusable object that boots the app for integration tests
namespace VideoGameCharactersAPI.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
    }
}
