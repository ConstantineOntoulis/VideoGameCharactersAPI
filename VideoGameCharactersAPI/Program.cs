using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using VideoGameCharactersAPI.Data;
using VideoGameCharactersAPI.Infrastructure;
using VideoGameCharactersAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

//configure how the API should read and verify incoming JWT tokens
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //configuration object containing all the checks the API will perform against a token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //Check who issued the token
        ValidateAudience = true, //Check who the token was intended for
        ValidateIssuerSigningKey = true, //Verify the token’s signature was created using the correct secret key
        ValidateLifetime = true, //Check whether the token has expired
        ValidIssuer = jwtIssuer, //This is the exact issuer value expected
        ValidAudience = jwtAudience, //This is the exact audience value I expect
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) //Use this shared secret key to verify that the token signature is real
    };                                                                                                                                  //Convert secret text into bytes
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOrAdmin", policy =>
        policy.RequireRole("User", "Admin"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

//Add services to the container.
builder.Services.AddControllers();

//registers the default problem-details service for standardized API errors
builder.Services.AddProblemDetails();

//registers the IExceptionHandler implementation
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//OpenAPI structure for eg. documentations
builder.Services.AddOpenApi();

//API Health Monitoring
builder.Services.AddHealthChecks();

//Add the database context to the application and tell Entity Framework to connect to SQL Server
builder.Services.AddDbContext<CharacterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register character service for dependency injection (one instance per request).
builder.Services.AddScoped<IVideoGameCharacterService, VideoGameService>();

var app = builder.Build();

//Applies missing migrations to the database,when the app starts(so that scema can exist in new environment)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CharacterDbContext>();
    dbContext.Database.Migrate();
}

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//Enable HTTPS redirection only during local development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

//Redirect requests from the root URL to the Scalar API documentation UI
app.MapGet("/", () => Results.Redirect("/scalar"));

app.MapHealthChecks("/health");
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { } //fallback for when test project cannot access Program.cs