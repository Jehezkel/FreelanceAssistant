using WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FreelancerClient>();
var app = builder.Build();

app.MapGet("/", (FreelancerClient client) =>
{
    client.AuthorizationRequest();
    return "HI";
});


app.Run();
