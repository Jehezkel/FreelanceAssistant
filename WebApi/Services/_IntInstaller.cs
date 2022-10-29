using WebApi.ApiClient;
using WebApi.Handlers;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public static class InternalServicesInstaller
{
    public static void InstallInternalServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<FreelancerConfig>(config.GetSection("Freelancer"));

        services.AddSingleton<RequestLoggingHandler>();
        services.AddHttpClient<IFreelancerClient, FreelancerClient>()
            .AddHttpMessageHandler<RequestLoggingHandler>();

        //Temp disable 
        // services.AddHostedService<RefreshManager>();

        services.Configure<MailSettings>(config.GetSection("MailSettings"));
        services.AddTransient<MailService>();

        services.AddSingleton<MailTemplateService>();

        services.AddTransient<TokenService>();
        services.AddScoped<IFLApiTokenRepository, FLApiTokenRepository>();
    }
}