using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using VideoGameCharactersAPI.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VideoGameCharactersAPI.Services;
using VideoGameCharacterAPI.Data;
using VideoGameCharactersApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CharacterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVideoGameCharacterService, VideoGameService>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

//configure how the API should read and verify incoming JWT tokens
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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
        }; //Convert secret text into bytes
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOrAdmin", policy =>
        policy.RequireRole("User", "Admin"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
