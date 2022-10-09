using webapi.Models;
using WebApi;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
services.Configure<FreelancerConfig>(config.GetSection("Freelancer"));
services.AddSingleton<FreelancerClient>();
services.AddHostedService<RefreshManager>();
services.Configure<MailSettings>(config.GetSection("MailSettings"));
services.AddTransient<MailService>();
var app = builder.Build();

app.MapGet("/Authorize", (FreelancerClient client) =>
{
    return Results.Json(new { authUrl = client.getAuthorizationUrl() });
});
app.MapPost("/VerifyCode", async (string code, FreelancerClient client) =>
{
    client.verifyCode(code);
});
app.MapGet("/VerifyCode", async (string code, FreelancerClient client) =>
{
    client.verifyCode(code);
    return Results.Redirect("/");
});
app.MapGet("/Test", (FreelancerClient client) =>
{
    // return Results.Json(new { authUrl = client.getAuthorizationUrl() });
    return Results.Redirect(client.getAuthorizationUrl());

});
app.MapGet("/Projects", (FreelancerClient client) => client.fetchProjects());
// app.MapGet("/Stop", async (RefreshManager rm) => await rm.StopAsync(new CancellationToken()));
// app.MapGet("/Start", async (RefreshManager rm) => await rm.StartAsync(new CancellationToken()));

app.Run();
