using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.DAL;
using WebApi.Models;

namespace WebApi.Services;

public static class ExternalServicesInstaller
{
    public static void AddExternalServices(this IServiceCollection services, IConfiguration config)
    {
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
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
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

        string corsPolicyName = config["CORS:PolicyName"];
        var allowedOrigins = config.GetSection("CORS:AllowedOrigins").Get<string[]>();
        services.AddCors(o => o.AddPolicy(name: corsPolicyName, policy =>
        {
            policy.WithOrigins(allowedOrigins);
            policy.AllowAnyHeader();
        }));
    }
    public static void UseExternalServices(this WebApplication app, IConfiguration config)
    {
        app.MapControllers();
        app.UseCors(config["CORS:PolicyName"]);
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
    }

}