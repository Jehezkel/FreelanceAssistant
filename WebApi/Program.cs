using WebApi;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Models;
using WebApi.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using WebApi.ApiClient;
using WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddDbContext<IFLDbContext, FLDbContext>(opt => opt.UseNpgsql(config.GetConnectionString("FLDbContext")));

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
services.AddSingleton<RequestLoggingHandler>();
services.AddHttpClient<IFreelancerClient, FreelancerClient>()
    .AddHttpMessageHandler<RequestLoggingHandler>();

services.AddSingleton<MailTemplateService>();
services.AddHttpContextAccessor();
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
///CORS Policy
string corsPolicyName = "localhost";
var allowedOrigins = config.GetSection("CORS:AllowedOrigins").Get<string[]>();
services.AddCors(o => o.AddPolicy(name: corsPolicyName, policy =>
{
    policy.WithOrigins(allowedOrigins);
    policy.AllowAnyHeader();
}));


var app = builder.Build();
app.MapControllers();
app.UseCors(corsPolicyName);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseAuthentication();

app.UseAuthorization();

app.Run();
