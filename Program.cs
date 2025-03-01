using ECommerceTask.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommerceTask.Application.Services;
using ECommerceTask.Application.Interfaces;
using ECommerceTask.Infrastructure.Repositories;
using ECommerceTask.Helpers;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();
builder.Services.AddSingleton<TokenGenerator>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings.GetValue<string>("Key");
var issuer = jwtSettings.GetValue<string>("Issuer");
var audience = jwtSettings.GetValue<string>("Audience");

if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new InvalidOperationException("JWT settings are not properly configured.");
}
Console.WriteLine("Checking JWT Configuration...");
Console.WriteLine($"JWT Key: {builder.Configuration["Jwt:Key"]}");
Console.WriteLine($"JWT Issuer: {builder.Configuration["Jwt:Issuer"]}");
Console.WriteLine($"JWT Audience: {builder.Configuration["Jwt:Audience"]}");

if (string.IsNullOrEmpty(builder.Configuration["Jwt:Key"]) ||
    string.IsNullOrEmpty(builder.Configuration["Jwt:Issuer"]) ||
    string.IsNullOrEmpty(builder.Configuration["Jwt:Audience"]))
{
    throw new InvalidOperationException("JWT settings are missing in configuration.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});






var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
