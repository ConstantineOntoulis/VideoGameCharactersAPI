using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VideoGameCharactersAPI.Dtos;

namespace VideoGameCharacterApi.Controllers
{
    //Defines base route for controller: /api/auth
    [Route("api/[controller]")]
    [ApiController]
    //IConfiguration injected so controller can read JWT settings & ILogger is used to record meaningful authentication events
    public class AuthController(IConfiguration configuration, ILogger<AuthController> logger) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            //Checks whether the provided credentials match the demo User account
            if (request.Username == "user" && request.Password == "user123")
            {
                //Log the successful authentication event
                logger.LogInformation(
                    "Successfully login for username {Username} with role {Role}.",
                    request.Username,
                    "User");

                return Ok(new LoginResponse
                {
                    Token = GenerateToken("user", "User"),
                    Role = "User"
                });
            }

            //Checks whether the provided credentials match the demo Admin account
            if (request.Username == "admin" && request.Password == "admin123")
            {
                //Log the successful authentication event
                logger.LogInformation(
                    "Succesful login for username {Username} with role {Role}.",
                    request.Username,
                    "Admin");

                return Ok(new LoginResponse
                {
                    Token = GenerateToken("admin", "Admin"),
                    Role = "Admin"
                });
            }
            //Log a failed login attempt
            logger.LogWarning(
                "Failed login attempt for username {Username}.",
                request.Username);

            //If neither credential pair matches, authentication fails
            return Unauthorized(new { message = "Invalid username or password." }); //HTTP 401 Unauthorized
        }

        //Creates and signs a JWT for the given username and role
        private string GenerateToken(string username, string role)
        {
            var key = configuration["Jwt:Key"]!;
            var issuer = configuration["Jwt:Issuer"]!;
            var audience = configuration["Jwt:Audience"]!;

            //Creates the claims that will be stored inside the JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            //Converts the configured key string into a cryptographic signing key
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //Defines the signing credentials for the token.HmacSha256 is the algorithm used to sign the token
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Creates the JWT object with issuer, audience, claims, expiry, and signing information
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            //Converts JWT object into the final compact token string that the client will send in the Authorization header
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}