using WebApi.ApiClient;
using WebApi.Handlers;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public static class InternalServicesInstaller
{
    public static void AddInternalServices(this IServiceCollection services, IConfiguration config)
    {

        //Temp disable 
        // services.AddHostedService<RefreshManager>();
        
        services.AddTransient<ITokenService,TokenService>();

        services.Configure<MailSettings>(config.GetSection("MailSettings"));
        services.AddTransient<MailService>();
        services.AddSingleton<MailTemplateService>();


        services.AddTransient<RequestLoggingHandler>();

        services.Configure<FreelancerConfig>(config.GetSection("Freelancer"));
        services.AddHttpClient<IFreelancerClient, FreelancerClient>()
            .AddHttpMessageHandler<RequestLoggingHandler>();

        services.AddScoped<IFLApiTokenRepository, FLApiTokenRepository>();
    }
}