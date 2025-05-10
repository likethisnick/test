using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestAndSurvey.DataAccess;
using TestAndSurvey.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDataProtection();
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://10.0.2.2:5219")
              .WithHeaders("Authorization", "Content-Type", "Accept")
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.WebHost.UseUrls("https://localhost:44308", "http://localhost:5219");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityCore<SurvefyUser>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<SurvefyDbContext>()
.AddSignInManager();

builder.Services.AddDbContext<SurvefyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
    throw new Exception("JWT key must be at least 32 characters long");

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.UseSecurityTokenValidators = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            NameClaimType = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/logout", async (SignInManager<SurvefyUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

app.MapGet("/pingauth", (HttpContext context) =>
{
    var token = context.Request.Headers["Authorization"].ToString();
    var isAuth = context.User.Identity?.IsAuthenticated ?? false;
    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    return Results.Json(new
    {
        token,
        userId,
        isAuth
    });
}).RequireAuthorization();
// Убери .RequireAuthorization() временно


app.MapGet("/getCurUserId", (ClaimsPrincipal user) =>
{
    var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
    return Results.Json(new { id });
}).RequireAuthorization();

app.MapControllers();
app.Run();
