using WebApi;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Models;
using WebApi.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
services.AddDbContext<FLDbContext>(opt => opt.UseNpgsql(config.GetConnectionString("FLDbContext")));
services.Configure<IdentityOptions>(opt =>
{
    opt.User.RequireUniqueEmail = true;
});
services.AddDefaultIdentity<AppUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<FLDbContext>();
services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    var jwtSection = config.GetSection("Authentication:Jwt");
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidIssuer = jwtSection["ValidIssuer"],
        ValidAudience = jwtSection["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]))
    };
});

services.Configure<FreelancerConfig>(config.GetSection("Freelancer"));
services.AddSingleton<FreelancerClient>();
services.AddSingleton<MailTemplateService>(s => new MailTemplateService());
//Temp disable 
// services.AddHostedService<RefreshManager>();

services.Configure<MailSettings>(config.GetSection("MailSettings"));
services.AddTransient<MailService>();
services.AddTransient<TokenService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    {
        Description = "Auth header using the bearer",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT"
                }
            },
            new List<string>()
        }
    });
});

services.AddControllers();

var app = builder.Build();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}










// app.MapGet("/Authorize", (FreelancerClient client) =>
// {
//     return Results.Json(new { authUrl = client.getAuthorizationUrl() });
// });
// app.MapPost("/VerifyCode", async (string code, FreelancerClient client) =>
// {
//     client.verifyCode(code);
// });
// app.MapGet("/VerifyCode", async (string code, FreelancerClient client) =>
// {
//     client.verifyCode(code);
//     return Results.Redirect("/");
// });
// app.MapGet("/Test", (FreelancerClient client) =>
// {
//     return Results.Redirect(client.getAuthorizationUrl());
// });
// app.MapGet("/Projects", (FreelancerClient client) => client.fetchProjects());
// app.MapGet("/auth/google-signin", (string code) => Results.Ok());

app.Run();
