using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
//EF core + authentication + swagger+ cors
services.AddExternalServices(config);
//Project services: mailservice/FL webapiclient
services.AddInternalServices(config);

var app = builder.Build();
//Use swagger/authentiication/cors
app.UseExternalServices(config);

app.Run();
