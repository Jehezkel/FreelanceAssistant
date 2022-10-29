using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
//EF core + authentication + swagger+ cors
services.InstallExternalServices(config);
//Project services: mailservice/FL webapiclient
services.InstallInternalServices(config);

var app = builder.Build();
//Use swagger/authentiication/cors
app.UseExternalServices(config);

app.Run();
